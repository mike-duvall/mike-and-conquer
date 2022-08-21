using System.Drawing;
using System.IO;

namespace mike_and_conquer_monogame.openra
{
    public interface ISpriteLoader
    {
        bool TryParseSprite(Stream s, out ISpriteFrame[] frames);
    }

    public interface ISpriteFrame
    {
        /// <summary>
        /// Size of the frame's `Data`.
        /// </summary>
        Size Size { get; }

        /// <summary>
        /// Size of the entire frame including the frame's `Size`.
        /// Think of this like a picture frame.
        /// </summary>
        Size FrameSize { get; }

        float2 Offset { get; }
        byte[] Data { get; }
        bool DisableExportPadding { get; }
    }



}