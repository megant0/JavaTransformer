using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaTransformer.UI.MainDesktop.Models.Common.DataTransferObject.Settings
{
    public class EditorSettings
    {
        public string Theme { get; set; } = "DarkPlus";
        public int FontSize { get; set; } = 14;
        public bool AutoSave { get; set; } = true;
        public int AutoSaveInterval { get; set; } = 300;
        public string DefaultEncoding { get; set; } = "UTF-8";
        public bool WordWrap { get; set; } = true;
        public bool LineNumbers { get; set; } = true;
        public bool AllowToggleOverstrikeMode { get; set; } = true;
        public bool EnableTextDragDrop { get; set; } = true;
        public bool ShowBoxForControlCharacters { get; set; } = true;
        public bool RightClickMovesCaret { get; set; } = true;
        public bool HighlightCurrentLine { get; set; } = true;

        public string LanguageDefault { get; set; } = ".cpp";
    }
}
