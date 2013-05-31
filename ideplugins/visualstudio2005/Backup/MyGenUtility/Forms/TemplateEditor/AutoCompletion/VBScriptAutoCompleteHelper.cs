using System;
using System.Collections.Generic;
using System.Text;

namespace MyGeneration.AutoCompletion
{
    class VBScriptAutoCompleteHelper : AutoCompleteHelper
    {
        public override bool IsCommentStyle(bool isTagged, int style)
        {
            return (style == (isTagged ? 82 : 1));
        }

        public override bool IsCodeStyle(bool isTagged, int style)
        {
            if (isTagged)
            {
                return (style == 86 || style == 84 || style == 81);
            }
            else
            {
                return (style == 7 || style == 3 || style == 0);
            }
        }
    }
}
