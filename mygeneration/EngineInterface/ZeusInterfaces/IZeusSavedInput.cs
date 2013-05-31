using System;
using System.Collections.Generic;
using System.Text;

namespace Zeus
{
    public interface IZeusSavedInput
    {
        string FilePath{ get; set; }
		IZeusSavedTemplateInput InputDataI { get; }
		bool Load();
		bool Save();
    }
}
