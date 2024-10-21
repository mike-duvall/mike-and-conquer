using mike_and_conquer_monogame.gamesprite;
using AnimationSequence = mike_and_conquer_monogame.util.AnimationSequence;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using XnaPoint = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace mike_and_conquer_monogame.gameview;

public class MCVView : UnitView
{
    private UnitSprite unitSprite;
    // private UnitSelectionCursor unitSelectionCursor;
    private UnitSelectionCursor unitSelectionCursor;
    private DestinationSquare destinationSquare;
    private bool drawDestinationSquare;

    //        enum AnimationSequences { STANDING_STILL, WALKING_UP, SHOOTING_UP };


    private bool showClickDetectionRectangle;
    private ClickDetectionRectangle clickDetectionRectangle;

    private int clickDetectionRectangleYOffset = 1;
    private int clickDetectionRectangleXOffset = 0;



    public const string SPRITE_KEY = "MCV";
    public const string SHP_FILE_NAME = "Shp\\mcv.shp";
    public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new GdiShpFileColorMapper();


    public MCVView(int unitId, int xInWorldCoordinates, int yInWorldCoordinates, int maxHealth, int health)
    {
        // this.myMCV = mcv;
        this.UnitId = unitId;
        this.XInWorldCoordinates = xInWorldCoordinates;
        this.YInWorldCoordinates = yInWorldCoordinates;
        this.MaxHealth = maxHealth;
        this.Health = health;


        unitSprite = new UnitSprite(SPRITE_KEY);
        // this.unitSprite.drawBoundingRectangle = true;
        unitSprite.drawBoundingRectangle = false;
        unitSprite.drawShadow = true;
        // this.mcvSelectionBox = new MCVSelectionBox();

        unitSize = new UnitSize(26, 26);
        // this.selectionCursorOffset = new XnaPoint(-18, -14);
        selectionCursorOffset = new XnaPoint(0, 2);


        unitSelectionCursor = new UnitSelectionCursor(
            this,
            37,
            33,
            8,
            8,
            36,
            18,
            16,
            XInWorldCoordinates,
            YInWorldCoordinates);

        // this.destinationSquare = new DestinationSquare();
        drawDestinationSquare = false;

        var animationSequence = new AnimationSequence(1);
        animationSequence.AddFrame(0);
        unitSprite.AddAnimationSequence(0, animationSequence);

        // showClickDetectionRectangle = true;
        showClickDetectionRectangle = false;
        clickDetectionRectangle = new ClickDetectionRectangle(
            this.XInWorldCoordinates + clickDetectionRectangleXOffset,
            this.YInWorldCoordinates + clickDetectionRectangleYOffset,
            this.unitSize.Width,
            this.unitSize.Height);

    }

    internal override Rectangle CreateClickDetectionRectangle()
    {
        return clickDetectionRectangle.GetRectangle();

    }



    internal override void Update(GameTime gameTime)
    {
        unitSelectionCursor.Update(gameTime);
        clickDetectionRectangle.Update(
            gameTime,
            XInWorldCoordinates + clickDetectionRectangleXOffset,
            YInWorldCoordinates + clickDetectionRectangleYOffset);

    }


    internal override void DrawNoShadow(GameTime gameTime, SpriteBatch spriteBatch)
    {
        // if (myMCV.Health <= 0)
        // {
        //     return;
        // }


        var worldCoordinatesAsVector2 = new Vector2(
            XInWorldCoordinates,
            YInWorldCoordinates);

        unitSprite.DrawNoShadow(gameTime, spriteBatch, worldCoordinatesAsVector2, SpriteSortLayers.UNIT_DEPTH);


        if (Selected)
        {
            unitSelectionCursor.DrawNoShadow(gameTime, spriteBatch, SpriteSortLayers.UNIT_DEPTH);
        }

        if (showClickDetectionRectangle)
        {
            clickDetectionRectangle.DrawNoShadow(gameTime, spriteBatch);
        }

    }


    internal override void DrawShadowOnly(GameTime gameTime, SpriteBatch spriteBatch)
    {
        // if (myMCV.Health <= 0)
        // {
        //     return;
        // }


        var worldCoordinatesAsVector2 = new Vector2(
            XInWorldCoordinates,
            YInWorldCoordinates);

        unitSprite.DrawShadowOnly(gameTime, spriteBatch, worldCoordinatesAsVector2, SpriteSortLayers.UNIT_DEPTH);


        if (Selected) unitSelectionCursor.DrawNoShadow(gameTime, spriteBatch, SpriteSortLayers.UNIT_DEPTH);
    }


    public void SetAnimate(bool animateFlag)
    {
        unitSprite.SetAnimate(animateFlag);
    }
}