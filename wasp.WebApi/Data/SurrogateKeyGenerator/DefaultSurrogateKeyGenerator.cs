using System;
using System.Linq;

using wasp.Core.Extensions;

namespace wasp.WebApi.Data.SurrogateKeyGenerator
{
    public class DefaultSurrogateKeyGenerator : BaseSurrogateKeyGenerator<string>
    {
        public override string GetNextKey()
        {
            char[] chars = (CurrentValue ?? "000000").Select(x => x).Reverse().ToArray();

            bool countNextUp = false;
            char[] newChars = new char[chars.Length];
            Range[] ranges = { 48..57, 65..90 };

            for (int index = 0; index < chars.Length; index++)
            {
                char character = chars[index];
                int p = character;

                if (index == 0 || countNextUp)
                {
                    countNextUp = false;
                    
                    IIndexedItem<Range>? r = ranges.SelectWithIndex().FirstOrDefault(w => w.Item.End.Value == p);

                    if (r is null)
                    {
                        p += 1;
                    }
                    else
                    {
                        int newIndex;
                        
                        if (ranges.Length == r.Index + 1)
                        {
                            newIndex = 0;
                            countNextUp = true;
                        }
                        else
                        {
                            newIndex = r.Index + 1;
                        }

                        p = ranges[newIndex].Start.Value;
                    }
                }

                newChars[chars.Length - index - 1] = (char)p;
            }
            
            return string.Concat(newChars);
        }
    }
}