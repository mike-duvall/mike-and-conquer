using System;
using mike_and_conquer_monogame.main;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Color = Microsoft.Xna.Framework.Color;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using SpriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects;
using Boolean = System.Boolean;


namespace mike_and_conquer_monogame.gameview;

public class UnitSelectionCursor
{
    private Texture2D boundingRectangle;


    // private GameObject myGameObject;
    private readonly UnitView myUnitView;

    private readonly Texture2D selectionCursorTexture;
    private Vector2 selectionCursorPosition;


    private Texture2D healthBarTexture;
    private Texture2D healthBarShadowTexture;
    private Vector2 healthBarPosition;

    private bool drawBoundingRectangle;

    // private Vector2 origin;
    private Vector2 middleOfUnitSelectionCursor;
    private Vector2 healthBarTextureOrigin;

    private float defaultScale = 1;

    private int overallWidth;
    private int overallHeight;

    private int selectionCursorPartWidth;
    private int selectionCursorPartHeight;
    private int healthBarWidth;


    private UnitSelectionCursor()
    {
    }


    public UnitSelectionCursor(
        UnitView unitView,
        int overallWidth,
        int overallHeight,
        int selectionCursorPartWidth,
        int selectionCursorPartHeight,
        int healthBarWidth,
        int healthBarX,
        int healthBarY,
        int locationX,
        int locationY)
    {
        myUnitView = unitView;
        this.overallWidth = overallWidth;
        this.overallHeight = overallHeight;
        this.selectionCursorPartWidth = selectionCursorPartWidth;
        this.selectionCursorPartHeight = selectionCursorPartHeight;
        this.healthBarWidth = healthBarWidth;

        // healthBarTextureOrigin = new Vector2(18, 16);
        healthBarTextureOrigin = new Vector2(healthBarX, healthBarY);
        middleOfUnitSelectionCursor  = new Vector2(overallWidth / 2, overallHeight / 2);


        selectionCursorTexture = InitializeSelectionCursor();

        boundingRectangle = InitializeBoundingRectangle();
        healthBarTexture = null;

        drawBoundingRectangle = false;

        healthBarShadowTexture = InitializeHealthBarShadow();
    }


    // TODO Delete these Fillxxxx methods
    // and replace with the Drawxxxx ones
    internal void FillHorizontalLine(Color[] data, int width, int height, int lineIndex, Color color)
    {
        var beginIndex = width * lineIndex;
        for (var i = beginIndex; i < beginIndex + width; ++i) data[i] = color;
    }

    internal void FillHorizontalLine(Color[] data, int width, int height, int lineIndex, Color color, int start,
        int end)
    {
        var beginIndex = width * lineIndex;
        var relativeStart = beginIndex + start;
        var relativeEnd = beginIndex + end;
        for (var i = beginIndex; i < beginIndex + width; ++i)
            if (i >= relativeStart && i <= relativeEnd)
                data[i] = color;
    }


    internal void FillVerticalLine(Color[] data, int width, int height, int lineIndex, Color color)
    {
        var beginIndex = lineIndex;
        for (var i = beginIndex; i < width * height; i += width) data[i] = color;
    }


    private void DrawHorizontalLine(Color[] data, Color color, int width, int height, int startX, int startY,
        int length)
    {
        if (startX + length > width) throw new Exception("Attempt to create line outside bounds of texture width");
        var beginIndex = startX + width * startY;

        for (var i = beginIndex; i < beginIndex + length; i++) data[i] = color;
    }

    private void DrawVerticalLine(Color[] data, Color color, int width, int height, int startX, int startY, int length)
    {
        var dataIndex = startX + width * startY;

        for (var i = 0; i < length; i++)
        {
            data[dataIndex] = color;
            dataIndex += width;
        }
    }


    internal Texture2D InitializeBoundingRectangle()
    {
        var rectangle = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, selectionCursorTexture.Width,
            selectionCursorTexture.Height);
        var data = new Color[rectangle.Width * rectangle.Height];
        FillHorizontalLine(data, rectangle.Width, rectangle.Height, 0, Color.White);
        FillHorizontalLine(data, rectangle.Width, rectangle.Height, rectangle.Height - 1, Color.White);
        FillVerticalLine(data, rectangle.Width, rectangle.Height, 0, Color.White);
        FillVerticalLine(data, rectangle.Width, rectangle.Height, rectangle.Width - 1, Color.White);
        var centerX = rectangle.Width / 2;
        var centerY = rectangle.Height / 2;
        var centerOffset = centerY * rectangle.Width + centerX;

        data[centerOffset] = Color.Red;

        rectangle.SetData(data);
        return rectangle;
    }


    private Texture2D InitializeHealthBar()
    {
        var healthBarHeight = 4;
        // var healthBarWidth = myUnitView.UnitSize.Width;


        var rectangle =
            new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, healthBarWidth, healthBarHeight);

        var data = new Color[rectangle.Width * rectangle.Height];

        var cncPalleteColorBlack = new Color(0, 255, 255, 255);
        var cncPalleteHealthColorGreen = new Color(4, 255, 255, 255);
        var cncPalleteHealthColorYellow = new Color(5, 255, 255, 255);
        var cncPalleteHealthColorRed = new Color(8, 255, 255, 255);

        var cncPalletteColorHealthBackground = new Color(240, 255, 255, 255);

        FillHorizontalLine(data, rectangle.Width, rectangle.Height, 0, cncPalleteColorBlack);

        FillHorizontalLine(data, rectangle.Width, rectangle.Height, 1, cncPalletteColorHealthBackground);
        FillHorizontalLine(data, rectangle.Width, rectangle.Height, 2, cncPalletteColorHealthBackground);


        FillHorizontalLine(data, rectangle.Width, rectangle.Height, 3, cncPalleteColorBlack);

        FillVerticalLine(data, rectangle.Width, rectangle.Height, 0, cncPalleteColorBlack);
        FillVerticalLine(data, rectangle.Width, rectangle.Height, healthBarWidth - 1, cncPalleteColorBlack);

        var nonBorderHealthBarWidth = healthBarWidth - 2;
        var maxHealth = myUnitView.MaxHealth;
        var ratio = (float)nonBorderHealthBarWidth / (float)maxHealth;
        var unitHealth = myUnitView.Health;
        var healthBarLength = (int)(unitHealth * ratio);

        // var healthColor = cncPalleteHealthColorGreen;
        // if (healthBarLength <= 17)
        // {
        //     healthColor = cncPalleteHealthColorYellow;
        // }
        // if (healthBarLength <= 8)
        // {
        //     healthColor = cncPalleteHealthColorRed;
        // }
        //
        //
        //
        // if (healthBarLength <= 2)
        //     healthColor = cncPalleteHealthColorRed;
        // else if (healthBarLength <= 5) healthColor = cncPalleteHealthColorYellow;
        //
        var healthPercentage = (float)unitHealth / maxHealth;

        Color healthColor;
        if (healthPercentage <= 0.25f)
        {
            healthColor = cncPalleteHealthColorRed;
        }
        else if (healthPercentage <= 0.5f)
        {
            healthColor = cncPalleteHealthColorYellow;
        }
        else
        {
            healthColor = cncPalleteHealthColorGreen;
        }
        FillHorizontalLine(data, rectangle.Width, rectangle.Height, 1, healthColor, 1, healthBarLength);
        FillHorizontalLine(data, rectangle.Width, rectangle.Height, 2, healthColor, 1, healthBarLength);

        rectangle.SetData(data);

        return rectangle;
    }

    internal Texture2D InitializeHealthBarShadow()
    {




        var healthBarHeight = 4;
        // int healthBarWidth = 12;  // This is hard coded for minigunner
        // var healthBarWidth = myUnitView.UnitSize.Width;

        var rectangle =
            new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, healthBarWidth, healthBarHeight);

        var data = new Color[rectangle.Width * rectangle.Height];

        var cncPalleteColorShadow = new Color(255, 255, 255, 255);

        FillHorizontalLine(data, rectangle.Width, rectangle.Height, 1, cncPalleteColorShadow);
        FillHorizontalLine(data, rectangle.Width, rectangle.Height, 2, cncPalleteColorShadow);

        rectangle.SetData(data);
        return rectangle;
    }


    private Texture2D CreateUnitSelectionTexture(int overallWidth, int overallHeight, int partWidth, int partHeight)
    {
        var unitSelectionTextureKey = "UnitSelectionTexture-width-" + overallWidth + "-height-" + overallHeight;


        var unitSelectionTexture = MikeAndConquerGame.instance.SpriteSheet.GetTextureForKey(unitSelectionTextureKey);

        if (unitSelectionTexture == null)
        {
            var cncPalleteColorWhite = new Color(255, 255, 255, 255);

            unitSelectionTexture =
                new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, overallWidth, overallHeight);

            var data = new Color[unitSelectionTexture.Width * unitSelectionTexture.Height];

            var startX = 0;
            var startY = 0;

            // top left
            DrawHorizontalLine(data, cncPalleteColorWhite, overallWidth, overallHeight, startX, startY, partWidth);
            DrawVerticalLine(data, cncPalleteColorWhite, overallWidth, overallHeight, startX, startY, partHeight);

            // bottom left
            startY = overallHeight - partHeight;
            DrawVerticalLine(data, cncPalleteColorWhite, overallWidth, overallHeight, startX, startY, partHeight);
            startY = overallHeight - 1;
            DrawHorizontalLine(data, cncPalleteColorWhite, overallWidth, overallHeight, startX, startY, partWidth);

            // top right
            startX = overallWidth - partWidth;
            startY = 0;
            DrawHorizontalLine(data, cncPalleteColorWhite, overallWidth, overallHeight, startX, startY, partWidth);
            startX = overallWidth - 1;
            DrawVerticalLine(data, cncPalleteColorWhite, overallWidth, overallHeight, startX, startY, partHeight);

            // bottom right
            startY = overallHeight - partHeight;
            DrawVerticalLine(data, cncPalleteColorWhite, overallWidth, overallHeight, startX, startY, partHeight);
            startX = overallWidth - partWidth;

            startY = overallHeight - 1;
            DrawHorizontalLine(data, cncPalleteColorWhite, overallWidth, overallHeight, startX, startY, partWidth);

            unitSelectionTexture.SetData(data);

            MikeAndConquerGame.instance.SpriteSheet.SetTextureForKey(unitSelectionTextureKey, unitSelectionTexture);
        }


        return unitSelectionTexture;
    }

    private Texture2D InitializeSelectionCursor()
    {
        // var unitSize = myUnitView.UnitSize;
        //
        // var width = unitSize.Width + 1;
        // var height = unitSize.Height - 4 + 1;

        // var width = unitSize.Width + 1;
        // var height = unitSize.Height - 4 + 1;


        // var horizontalLength = unitSize.Width / 5 + 1;
        // var verticalLength = unitSize.Height / 5 + 1;

        // var horizontalLength = width / 5 + 1;
        // var verticalLength = height / 5 + 1;



        return CreateUnitSelectionTexture(overallWidth, overallHeight, selectionCursorPartWidth, selectionCursorPartHeight);
    }


    public void Update(GameTime gameTime)
    {
        if (healthBarTexture != null) healthBarTexture.Dispose();
        healthBarTexture = InitializeHealthBar();

        var selectionCursorOffset = myUnitView.SelectionCursorOffset;
        // GameWorldLocation gameWorldLocation = myGameObject.GameWorldLocation;
        //
        //
        // selectionCursorPosition = new Vector2(
        //     gameWorldLocation.WorldCoordinatesAsVector2.X + selectionCursorOffset.X,
        //     gameWorldLocation.WorldCoordinatesAsVector2.Y + selectionCursorOffset.Y);


        selectionCursorPosition = new Vector2(
            myUnitView.XInWorldCoordinates + selectionCursorOffset.X,
            myUnitView.YInWorldCoordinates + selectionCursorOffset.Y);


        healthBarPosition = selectionCursorPosition;
        healthBarPosition.Y -= 4;
    }

    internal void DrawNoShadow(GameTime gameTime, SpriteBatch spriteBatch, float layerDepth)
    {
        spriteBatch.Draw(selectionCursorTexture, selectionCursorPosition, null, Color.White, 0f, middleOfUnitSelectionCursor, defaultScale,
            SpriteEffects.None, layerDepth);
        // if (drawBoundingRectangle)
        // {
        //     
        //     spriteBatch.Draw(boundingRectangle, selectionCursorPosition, null, Color.White, 0f, origin, defaultScale, SpriteEffects.None, 0f);
        // }

        spriteBatch.Draw(healthBarTexture, healthBarPosition, null, Color.White, 0f, healthBarTextureOrigin, defaultScale,
            SpriteEffects.None, layerDepth);
    }

    internal void DrawShadowOnly(GameTime gameTime, SpriteBatch spriteBatch, float layerDepth)
    {
        spriteBatch.Draw(healthBarShadowTexture, healthBarPosition, null, Color.White, 0f, healthBarTextureOrigin, defaultScale,
            SpriteEffects.None, layerDepth);

    }
}