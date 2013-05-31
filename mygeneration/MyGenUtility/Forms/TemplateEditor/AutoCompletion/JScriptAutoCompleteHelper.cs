using System;
using System.Collections.Generic;
using System.Text;

namespace MyGeneration.AutoCompletion
{
    class JScriptAutoCompleteHelper : AutoCompleteHelper
    {
        public override bool IsCommentStyle(bool isTagged, int style)
        {
            return (style == (isTagged ? 58 : 2));
        }

        public override bool IsCodeStyle(bool isTagged, int style)
        {
            if (isTagged)
            {
                return (style == 62 || style == 61 || style == 56 || style == 55);
            }
            else
            {
                return (style == 11 || style == 5 || style == 0);
            }
        }
    }
}
