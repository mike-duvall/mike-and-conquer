namespace mike_and_conquer_monogame.gamesprite
{
    public class NodShpFileColorMapper : ShpFileColorMapper
    {


        public override int MapColorIndex(int index)
        {
            if (index == 176)
                return 161;

            if (index == 177)
                return 200;


            if (index == 178)
                return 201;

            if (index == 179)
                return 202;

            if (index == 180)
                return 204;

            if (index == 181)
                return 205;

            if (index == 182)
                return 206;

            if (index == 183)
                return 12;

            if (index == 184)
                return 201;

            if (index == 185)
                return 202;

            if (index == 186)
                return 203;

            if (index == 187)
                return 204;

            if (index == 188)
                return 205;

            if (index == 189)
                return 115;

            if (index == 190)
                return 198;

            if (index == 191)
                return 114;

            return index;

        }
    }
}
