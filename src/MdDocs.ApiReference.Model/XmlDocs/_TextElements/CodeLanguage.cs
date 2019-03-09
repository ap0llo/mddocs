namespace Grynwald.MdDocs.ApiReference.Model.XmlDocs
{
    public enum CodeLanguage
    {
        None = 0,
        CSharp,
        CPlusPlus,
        C,
        FSharp,
        Javascript,
        VisualBasic,
        XML,
        HTML,
        XAML,
        SQL,
        Python,
        Powershell,
        Batch,

        /// <summary>
        /// Supported for backwards compatibility with SandCastle,
        /// see http://ewsoftware.github.io/XMLCommentsGuide/html/1abd1992-e3d0-45b4-b43d-91fcfc5e5574.htm
        /// </summary>
        VisualBasicScript,

        /// <summary>
        /// Supported for backwards compatibility with SandCastle,
        /// see http://ewsoftware.github.io/XMLCommentsGuide/html/1abd1992-e3d0-45b4-b43d-91fcfc5e5574.htm
        /// </summary>
        JSharp,

        /// <summary>
        /// Supported for backwards compatibility with SandCastle,
        /// see http://ewsoftware.github.io/XMLCommentsGuide/html/1abd1992-e3d0-45b4-b43d-91fcfc5e5574.htm
        /// </summary>
        JScriptDotNet
    }

}
