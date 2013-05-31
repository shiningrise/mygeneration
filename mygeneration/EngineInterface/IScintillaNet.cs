using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Scintilla;
using Scintilla.Forms;
using Scintilla.Configuration;
using Scintilla.Configuration.SciTE;

namespace MyGeneration
{
    public interface IScintillaNet
    {		
        void AddStyledText(int length, byte[] s);
        /*void GetStyledText(ref TextRange tr);
        int FindText(int searchFlags, ref TextToFind ttf);
        int FormatRange(bool bDraw, ref RangeToFormat oRangeToFormat);
        int GetTextRange(ref TextRange tr);*/
        char CharAt(int position);
        IntPtr DocPointer();
        IntPtr CreateDocument();
        void AddRefDocument(IntPtr pDoc);
        void ReleaseDocument(IntPtr pDoc);
        void AssignCmdKey(System.Windows.Forms.Keys keyDefinition, uint sciCommand);
        void ClearCmdKey(System.Windows.Forms.Keys keyDefinition);
        string GetText();
        int CodePage {set; }
        void SetFoldMarginColor(bool useSetting, Color back);
        void SetFoldMarginHiColor(bool useSetting, Color fore);
        void SetSelectionBackground(bool useSetting, Color back);
        void StyleSetFore(int style, Color fore);
        void StyleSetBack(int style, Color back);
        string ConfigurationLanguage { set; get; }
        int SafeBraceMatch(int position);
        void AddShortcuts(Form parentForm);
        void AddShortcuts(Menu.MenuItemCollection m);
        void AddShortcuts(ToolStripItemCollection m);
        void AddIgnoredKey(System.Windows.Forms.Shortcut shortcutkey);
        void AddIgnoredKey(System.Windows.Forms.Keys shortcutkey);
        void ClearIgnoredKeys(); 
        bool PreProcessMessage(ref Message m);
        string Text { get; set; }
        void AddLastLineEnd();
        void StripTrailingSpaces();
        bool PositionIsOnComment(int position);
        bool PositionIsOnComment(int position, int lexer);
        void ExpandAllFolds();
        void CollapseAllFolds();
        string GetWordFromPosition(int position);
        int BaseStyleAt(int pos);
        bool CanRedo { get; }
        bool IsAutoCActive { get; }
        bool CanPaste { get; }
        bool CanUndo { get; }
        bool IsCallTipActive { get; }
        int Length { get; }
        int CurrentPos { get; set; }
        int Anchor_ { get; set; }
        bool IsUndoCollection { get; set; }
        int EndStyled { get; }
        bool IsBufferedDraw { get; set; }
        int TabWidth { get; set; }
        int SelectionAlpha { get; set; }
        bool IsSelEOLFilled { get; set; }
        int CaretPeriod { get; set; }
        int StyleBits { get; set; }
        int MaxLineState { get; }
        bool IsCaretLineVisible { get; set; }
        int CaretLineBackgroundColor { get; set; }
        int AutoCSeparator { get; set; }
        bool IsAutoCCancelAtStart { get; set; }
        bool IsAutoCChooseSingle { get; set; }
        bool IsAutoCIgnoreCase { get; set; }
        bool IsAutoCAutoHide { get; set; }
        bool IsAutoCDropRestOfWord { get; set; }
        int AutoCTypeSeparator { get; set; }
        int AutoCMaxWidth { get; set; }
        int AutoCMaxHeight { get; set; }
        int Indent { get; set; }
        bool IsUseTabs { get; set; }
        bool IsHScrollBar { get; set; }
        Scintilla.Enums.IndentationGuideType IndentationGuide { get; set; }
        int HighlightGuide { get; set; }
        int CaretFore { get; set; }
        bool IsUsePalette { get; set; }
        bool IsReadOnly { get; set; }
        int SelectionStart { get; set; }
        int SelectionEnd { get; set; }
        int PrintMagnification { get; set; }
        int PrintColorMode { get; set; }
        int FirstVisibleLine { get; }
        int LineCount { get; }
        int MarginLeft { get; set; }
        int MarginRight { get; set; }
        bool IsModify { get; }
        int TextLength { get; }
        int DirectFunction { get; }
        int DirectPointer { get; }
        bool IsOvertype { get; set; }
        int CaretWidth { get; set; }
        int TargetStart { get; set; }
        int TargetEnd { get; set; }
        int SearchFlags { get; set; }
        bool IsTabIndents { get; set; }
        bool BackspaceUnIndents { get; set; }
        int MouseDwellTime { get; set; }
        int WrapMode { get; set; }
        int WrapVisualFlags { get; set; }
        int WrapVisualFlagsLocation { get; set; }
        int WrapStartIndent { get; set; }
        int LayoutCache { get; set; }
        int ScrollWidth { get; set; }
        bool IsEndAtLastLine { get; set; }
        bool IsVScrollBar { get; set; }
        bool IsTwoPhaseDraw { get; set; }
        bool IsViewEOL { get; set; }
        int EdgeColumn { get; set; }
        int EdgeMode { get; set; }
        int EdgeColor { get; set; }
        int LinesOnScreen { get; }
        bool IsSelectionIsRectangle { get; }
        int Zoom { get; set; }
        int ModEventMask { get; set; }
        bool IsFocus { get; set; }
        int Status { get; set; }
        bool IsMouseDownCaptures { get; set; }
        int Cursor_ { get; set; }
        int ControlCharSymbol { get; set; }
        int XOffset { get; set; }
        int PrintWrapMode { get; set; }
        int HotspotActiveFore { get; set; }
        int HotspotActiveBack { get; set; }
        bool IsHotspotActiveUnderline { get; set; }
        bool IsHotspotSingleLine { get; set; }
        int SelectionMode { get; set; }
        bool IsCaretSticky { get; set; }
        bool IsPasteConvertEndings { get; set; }
        int CaretLineBackgroundAlpha { get; set; }
        int Lexer { get; set; }
        int StyleBitsNeeded { get; }
        void AddText(string text);
        void InsertText(int pos, string text);
        void ClearAll();
        void ClearDocumentStyle();
        void Redo();
        void SelectAll();
        void SetSavePoint();
        int MarkerLineFromHandle(int handle);
        void MarkerDeleteHandle(int handle);
        int PositionFromPoint(int x, int y);
        int PositionFromPointClose(int x, int y);
        void GotoLine(int line);
        void GotoPos(int pos);
        string GetCurLine();
        void StartStyling(int pos, int mask);
        void SetStyling(int length, int style);
        void MarkerSetForegroundColor(int markerNumber, int fore);
        void MarkerSetBackgroundColor(int markerNumber, int back);
        int MarkerAdd(int line, int markerNumber);
        void MarkerDelete(int line, int markerNumber);
        void MarkerDeleteAll(int markerNumber);
        int MarkerGet(int line);
        int MarkerNext(int lineStart, int markerMask);
        int MarkerPrevious(int lineStart, int markerMask);
        void MarkerDefinePixmap(int markerNumber, string pixmap);
        void MarkerAddSet(int line, int set);
        void MarkerSetAlpha(int markerNumber, int alpha);
        void StyleResetDefault();
        string StyleGetFont(int style);
        void SetSelectionForeground(bool useSetting, int fore);
        void SetSelectionBackground(bool useSetting, int back);
        void ClearAllCmdKeys();
        void SetStylingEx(string styles);
        void BeginUndoAction();
        void EndUndoAction();
        void SetWhiteSpaceForeground(bool useSetting, int fore);
        void SetWhiteSpaceBackground(bool useSetting, int back);
        void AutoCShow(int lenEntered, string itemList);
        void AutoCCancel();
        int AutoCPosStart();
        void AutoCComplete();
        void AutoCStops(string characterSet);
        void AutoCSelect(string text);
        void UserListShow(int listType, string itemList);
        void RegisterImage(int type, string xpmData);
        void ClearRegisteredImages();
        void SetSelection(int start, int end);
        string GetSelectedText();
        void HideSelection(bool normal);
        int PointXFromPosition(int pos);
        int PointYFromPosition(int pos);
        int LineFromPosition(int pos);
        int PositionFromLine(int line);
        void LineScroll(int columns, int lines);
        void ScrollCaret();
        void ReplaceSelection(string text);
        void Null();
        void EmptyUndoBuffer();
        void Undo();
        void Cut();
        void Copy();
        void Paste();
        void Clear();
        void SetText(string text);
        int ReplaceTarget(string text);
        int ReplaceTargetRE(string text);
        int SearchInTarget(string text);
        void CallTipShow(int pos, string definition);
        void CallTipCancel();
        int CallTipPosStart();
        void CallTipSetHlt(int start, int end);
        int VisibleFromDocLine(int line);
        int DocLineFromVisible(int lineDisplay);
        int WrapCount(int line);
        void ShowLines(int lineStart, int lineEnd);
        void HideLines(int lineStart, int lineEnd);
        void ToggleFold(int line);
        void EnsureVisible(int line);
        void SetFoldFlags(int flags);
        void EnsureVisibleEnforcePolicy(int line);
        int WordStartPosition(int pos, bool onlyWordCharacters);
        int WordEndPosition(int pos, bool onlyWordCharacters);
        int TextWidth(int style, string text);
        int TextHeight(int line);
        void AppendText(string text);
        void TargetFromSelection();
        void LinesJoin();
        void LinesSplit(int pixelWidth);
        void SetFoldMarginColor(bool useSetting, int back);
        void SetFoldMarginHiColor(bool useSetting, int fore);
        void LineDown();
        void LineDownExtend();
        void LineUp();
        void LineUpExtend();
        void CharLeft();
        void CharLeftExtend();
        void CharRight();
        void CharRightExtend();
        void WordLeft();
        void WordLeftExtend();
        void WordRight();
        void WordRightExtend();
        void Home();
        void HomeExtend();
        void LineEnd();
        void LineEndExtend();
        void DocumentStart();
        void DocumentStartExtend();
        void DocumentEnd();
        void DocumentEndExtend();
        void PageUp();
        void PageUpExtend();
        void PageDown();
        void PageDownExtend();
        void EditToggleOvertype();
        void Cancel();
        void DeleteBack();
        void Tab();
        void BackTab();
        void NewLine();
        void FormFeed();
        void VCHome();
        void VCHomeExtend();
        void ZoomIn();
        void ZoomOut();
        void DelWordLeft();
        void DelWordRight();
        void LineCut();
        void LineDelete();
        void LineTranspose();
        void LineDuplicate();
        void Lowercase();
        void Uppercase();
        void LineScrollDown();
        void LineScrollUp();
        void DeleteBackNotLine();
        void HomeDisplay();
        void HomeDisplayExtend();
        void LineEndDisplay();
        void LineEndDisplayExtend();
        void HomeWrap();
        void HomeWrapExtend();
        void LineEndWrap();
        void LineEndWrapExtend();
        void VCHomeWrap();
        void VCHomeWrapExtend();
        void LineCopy();
        void MoveCaretInsideView();
        int LineLength(int line);
        void BraceHighlight(int pos1, int pos2);
        void BraceBadLight(int pos);
        int BraceMatch(int pos);
        void SearchAnchor();
        int SearchNext(int flags, string text);
        int SearchPrevious(int flags, string text);
        void UsePopup(bool allowPopUp);
        void WordPartLeft();
        void WordPartLeftExtend();
        void WordPartRight();
        void WordPartRightExtend();
        void SetVisiblePolicy(int visiblePolicy, int visibleSlop);
        void DelLineLeft();
        void DelLineRight();
        void ChooseCaretX();
        void GrabFocus();
        void SetXCaretPolicy(int caretPolicy, int caretSlop);
        void SetYCaretPolicy(int caretPolicy, int caretSlop);
        void ParaDown();
        void ParaDownExtend();
        void ParaUp();
        void ParaUpExtend();
        int PositionBefore(int pos);
        int PositionAfter(int pos);
        void CopyRange(int start, int end);
        void CopyText(string text);
        int GetLineSelectionStartPosition(int line);
        int GetLineSelectionEndPosition(int line);
        void LineDownRectExtend();
        void LineUpRectExtend();
        void CharLeftRectExtend();
        void CharRightRectExtend();
        void HomeRectExtend();
        void VCHomeRectExtend();
        void LineEndRectExtend();
        void PageUpRectExtend();
        void PageDownRectExtend();
        void StutteredPageUp();
        void StutteredPageUpExtend();
        void StutteredPageDown();
        void StutteredPageDownExtend();
        void WordLeftEnd();
        void WordLeftEndExtend();
        void WordRightEnd();
        void WordRightEndExtend();
        void SetCharsDefault();
        int AutoCGetCurrent();
        void Allocate(int bytes);
        string TargetAsUTF8();
        void SetLengthForEncode(int bytes);
        string EncodedFromUTF8(string utf8);
        int FindColumn(int line, int column);
        void ToggleCaretSticky();
        void SelectionDuplicate();
        void StartRecord();
        void StopRecord();
        void Colorize(int start, int end);
        void LoadLexerLibrary(string path);
        string GetProperty(string key);
        string GetPropertyExpanded(string key);
        int StyleAt(int pos);
        int MarginTypeN(int margin);
        int MarginWidthN(int margin);
        int MarginMaskN(int margin);
        bool MarginSensitiveN(int margin);
        int StyleGetFore(int style);
        int StyleGetBack(int style);
        bool StyleGetBold(int style);
        bool StyleGetItalic(int style);
        int StyleGetSize(int style);
        bool StyleGetEOLFilled(int style);
        bool StyleGetUnderline(int style);
        int StyleGetCase(int style);
        int StyleGetCharacterSet(int style);
        bool StyleGetVisible(int style);
        bool StyleGetChangeable(int style);
        bool StyleGetHotSpot(int style);
        int IndicGetFore(int indic);
        int LineState(int line);
        int LineIndentation(int line);
        int LineIndentPosition(int line);
        int Column(int pos);
        int LineEndPosition(int line);
        int FoldLevel(int line);
        int LastChild(int line, int level);
        int FoldParent(int line);
        bool LineVisible(int line);
        bool FoldExpanded(int line);
        int PropertyInt(string key);
        void MarginTypeN(int margin, int marginType);
        void MarginWidthN(int margin, int pixelWidth);
        void MarginMaskN(int margin, int mask);
        void MarginSensitiveN(int margin, bool sensitive);
        void StyleClearAll();
        void StyleSetFore(int style, int fore);
        void StyleSetBack(int style, int back);
        void StyleSetBold(int style, bool bold);
        void StyleSetItalic(int style, bool italic);
        void StyleSetSize(int style, int sizePoints);
        void StyleSetFont(int style, string fontName);
        void StyleSetEOLFilled(int style, bool filled);
        void StyleSetUnderline(int style, bool underline);
        void StyleSetHotSpot(int style, bool hotspot);
        void StyleSetVisible(int style, bool visible);
        void WordChars(string characters);
        void LineState(int line, int state);
        void StyleSetChangeable(int style, bool changeable);
        void AutoCSetFillUps(string characterSet);
        void LineIndentation(int line, int indentSize);
        void CallTipSetBack(int back);
        void CallTipSetFore(int fore);
        void CallTipSetForeHlt(int fore);
        void CallTipUseStyle(int tabSize);
        void FoldLevel(int line, int level);
        void FoldExpanded(int line, bool expanded);
        void WhitespaceChars(string characters);
        void Property(string key, string value);
        void KeyWords(int keywordSet, string keyWords);
        void LexerLanguage(string language);
    }
}
