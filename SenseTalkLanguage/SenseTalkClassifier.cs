namespace TestPlant.SenseTalkLanguage
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.Language.StandardClassification;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Classification;
    using System.Linq;
    using System.Text.RegularExpressions;

    public enum State
    {
        None,
        angles,
        blockcomment,
        blockquote,
        keyword,
        linecomment,
        number,
        operator_,
        punctuation,
        quote,
        qualifier,
        var,
    };

    public struct MultiLineToken
    {
        //Classification used by the token
        public IClassificationType Classification;
        //Tracked span of token
        public ITrackingSpan Tracking;
        //Version of text when Tracking was created
        public ITextVersion Version;
    }

    //Token returned by the Scan function (see Scan below)
    public class Token
    {
        //Position in text
        public int StartIndex;
        public int Length;
        public State TokenType;
        public State ScanState; // State.None if token is fully scanned, else token is partially scanned (does not end
        //in scanned text block). This state can be used to continue scanning process with adjacent text block.

        public Token(int startIndex, int length, State tokenType, State scanState=State.None)
        {
            StartIndex = startIndex;
            Length = length;
            TokenType = tokenType;
            ScanState = scanState;
        }
    }

    public class SenseTalkClassifier : IClassifier
    {
        protected List<MultiLineToken> _multiLineTokens;
        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;
        IClassificationTypeRegistryService _classificationTypeRegistry;

        public SenseTalkClassifier(IClassificationTypeRegistryService registry)
        {
            _multiLineTokens = new List<MultiLineToken>();
            this._classificationTypeRegistry = registry;
        }

        //Invoke ClassificationChanged that causes the editor to re-classify the specified span 
        protected void Invalidate(SnapshotSpan span)
        {
            if (ClassificationChanged != null)
                ClassificationChanged(this, new ClassificationChangedEventArgs(span));
        }

        // This method returns the first token found in text, starting from startIndex until startIndex + length
        // startTokenId and startState are necessary only to continue the scan with a partial-scanned token 
        protected Token Scan(string text, int startIndex, int length, State startTokenId = State.None, State startState = State.None)
        {
            Token token = null;
            State state = startState;

            if (text.Length == 0)
                return token;

            int startComment = startIndex;
            int startQuote = startIndex;
            string word = "";

            int eom = startIndex + length - 1;
            char prev = new char();
            for (int i = startIndex; i < text.Length; i++)
            {
                char c = text[i];
                if (i > startIndex)
                    prev = text[i - 1];

                if (state == State.blockcomment)
                {
                    if (i == eom)
                    {
                        return new Token(startComment, i-startComment+1, state, state);
                    }
                    else if ((c == ')' && prev == '*'))
                    {
                        return new Token(startComment, i-startComment+1, state);
                    }
                    continue;
                }
                if (state == State.blockquote)
                {
                    if (i == eom)
                    {
                        return new Token(startQuote, i - startQuote + 1, state, state);
                    }
                    else if (c == '>' && prev == '>')
                    {
                        return new Token(startQuote, i - startQuote + 1, state);
                    }
                    continue;
                }
                if (state == State.quote)
                {
                    if (c == '"')
                    {
                        return new Token(startQuote, i-startQuote+1, state);
                    }
                    continue;
                }

                if ((c == '/' && prev == '/') || (c == '-' && prev == '-'))
                {
                    int eol = text.IndexOf('\n', i);
                    if (eol != -1)
                        return new Token(i-1, eol-i + 1, State.linecomment);
                }

                if (c == '*' && prev == '(')
                {
                    state = State.blockcomment;
                    startComment = i - 1;
                    continue;
                }
                if (c == '<' && prev == '<')
                {
                    state = State.blockquote;
                    startQuote = i - 1;
                    continue;
                }
                if (c == '"')
                {
                    startQuote = i;
                    state = State.quote;
                    continue;
                }
                if (Char.IsLetterOrDigit(c))
                {
                    word += c;
                }
                else
                {
                    int len = word.Length;
                    if (len > 0) 
                    {
                        word = word.ToLower();
                        double d;
                        if (double.TryParse(word, out d))
                            return new Token(i - len, len, State.number);

                        if (KeyWords.Commands.Contains(word))                 
                            return new Token(i-len, len, State.keyword);
                        if (KeyWords.Functions.Contains(word))
                            return new Token(i - len, len, State.keyword);
                        if (KeyWords.TypeTextKeywords.Contains(word))
                            return new Token(i-len, len, State.qualifier);
                        if (KeyWords.Operators.Contains(word))
                            return new Token(i - len, len, State.operator_);
                        if (KeyWords.Operators2.Contains(word))
                            return new Token(i - len, len, State.operator_);

                        // Unknown word - assume a variable
                        return new Token(i - len, len, State.var);
                    }

                }
            }

            return token;
        }

        public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
        {
            var result = new List<ClassificationSpan>();

            bool isInsideMultiLine = false;

            //Scan for all known multi-line tokens, checking for current span intersection 
            for (var i = _multiLineTokens.Count - 1; i >= 0; i--)
            {
                var multiSpan = _multiLineTokens[i].Tracking.GetSpan(span.Snapshot);
                //Check if the span of the multi-line token is collapsed (zero length), and if true
                //remove it from the list
                if (multiSpan.Length == 0)
                    _multiLineTokens.RemoveAt(i);
                else
                {
                    if (span.IntersectsWith(multiSpan))
                    {
                        isInsideMultiLine = true;
                        //check if multi-line token is changed by comparing version of current 
                        //span with version on which token is found 
                        if (span.Snapshot.Version != _multiLineTokens[i].Version)
                        {
                            //if text inside multi-line token is changed, force the re-classication 
                            //of the whole multi-line token span and remove it from the list
                            _multiLineTokens.RemoveAt(i);
                            Invalidate(multiSpan);
                        }
                        else
                        {
                            //if no change, re-classify whole span with using current classification 
                            //(counterwise we loose actual classification)
                            result.Add(new ClassificationSpan(multiSpan, _multiLineTokens[i].Classification));
                        }
                    }
                }
            }

            if (!isInsideMultiLine)
            {
                //Start / end position (absolute) of current token 
                int startPosition, snapshotStartPosition;
                int endPosition, snapshotEndPosition;
                int currentOffset = 0; //Offset relative to the current span
                string currentText = span.GetText();

                Token token = null;
                bool useSnapshot = false;

                //Scan the current span for all tokens.
                do
                {
                    //Calculate current absolute start position
                    startPosition = span.Start.Position + currentOffset;
                    endPosition = startPosition;

                    token = Scan(currentText, currentOffset, currentText.Length - currentOffset, State.None, State.None);
                    if (token != null)
                    {
                        // Found a token in currentText at token.StartIndex
                        endPosition = token.StartIndex + token.Length;
                        startPosition = token.StartIndex;

                        //if token.State != 0, that means that token is not ending in current span,
                        //so continue read text until the token is fully read
                        snapshotStartPosition = span.Start.Position + startPosition;
                        snapshotEndPosition = span.Start.Position + endPosition;
                        while (token != null && token.ScanState != State.None && snapshotEndPosition < span.Snapshot.Length)
                        {
                            //Get 1024 text block size (or less, if the remaining text is shorter)                                                      
                            int textSize = Math.Min(span.Snapshot.Length - snapshotEndPosition, 1024);
                            string currentText1 = span.Snapshot.GetText(snapshotEndPosition, textSize);
                            //Scan for next token, starting from previous Scan State
                            token = Scan(currentText1, 0, currentText1.Length, token.TokenType, token.ScanState);
                            if (token != null)
                            {
                                snapshotEndPosition += token.Length; // move to new end       
                                useSnapshot = true; // classification span must be on the snapshot
                            }
                        }

                        bool multiLineToken = false;

                        //Assign classification type based on TokenId
                        IClassificationType classification = null;
                        switch (token.TokenType)
                        {
                            case State.keyword: //keyword
                                classification = _classificationTypeRegistry.GetClassificationType("sensetalk.keyword");
                                break;
                            case State.blockcomment: //multi-line-comment
                                classification = _classificationTypeRegistry.GetClassificationType("sensetalk.comment");
                                //mark this token as multiline
                                if (useSnapshot)
                                    multiLineToken = true;
                                break;
                            case State.quote:
                                classification = _classificationTypeRegistry.GetClassificationType("sensetalk.quote");
                                break;
                            case State.blockquote:
                                classification = _classificationTypeRegistry.GetClassificationType("sensetalk.quote");
                                if (useSnapshot)
                                    multiLineToken = true;
                                break;
                            case State.linecomment:
                                classification = _classificationTypeRegistry.GetClassificationType("sensetalk.comment");
                                break;
                            case State.number:
                                classification = _classificationTypeRegistry.GetClassificationType("sensetalk.number");
                                break;
                            case State.operator_:
                                classification = _classificationTypeRegistry.GetClassificationType("sensetalk.operator");
                                break;
                            case State.qualifier:
                                classification = _classificationTypeRegistry.GetClassificationType("sensetalk.qualifier");
                                break;
                            case State.var:
                                classification = _classificationTypeRegistry.GetClassificationType("sensetalk.var");
                                break;
                        }

                        //Create and append classification span ////span.Start.Position, (endPosition - (startPosition + span.Start.Position))) :
                        var tokenSpan = useSnapshot ? new SnapshotSpan(span.Snapshot, snapshotStartPosition, snapshotEndPosition - snapshotStartPosition) :
                                        new SnapshotSpan(span.Start + startPosition, (endPosition - startPosition));
                        
                        result.Add(new ClassificationSpan(tokenSpan, classification));

                        //All multi-line tokens will be saved in a list and tracked. This will automatically
                        //update the start / end position of token when the text buffer is changed.
                        if (multiLineToken)
                        {
                            //Ensure that do not already exists into the list
                            if (!_multiLineTokens.Any(a => a.Tracking.GetSpan(span.Snapshot).Span == tokenSpan.Span))
                            {
                                _multiLineTokens.Add(new MultiLineToken()
                                {
                                    Classification = classification,
                                    Version = span.Snapshot.Version,
                                    Tracking = span.Snapshot.CreateTrackingSpan(tokenSpan.Span, SpanTrackingMode.EdgeExclusive)
                                });

                                //If token length exceeds current span length, i need to invalidate and re-classify 
                                //the remaining text
                                if (tokenSpan.End > span.End)
                                    Invalidate(new SnapshotSpan(span.End + 1, tokenSpan.End));
                            }
                        }

                        currentOffset = endPosition; //+= (token.StartIndex + token.Length);
                    }
                }
                //Continue the loop until all span is tokenized, or no more token are found.
                while (token != null && currentOffset < span.Snapshot.Length);//currentText.Length);
            }

            return result;
        }

    }
}