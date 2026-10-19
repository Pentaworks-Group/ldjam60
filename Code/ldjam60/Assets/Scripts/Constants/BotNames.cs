using System;
using System.Collections.Generic;

using GameFrame.Core.Extensions;

namespace Assets.Scripts.Constants
{
    public static class BotNames
    {
        private readonly static List<String> botNames = new List<String>()
        {
            "Dick Hurts",
            "Hugh Mongus",
            "Ben Dover",
            "Mike Rotch",
            "Seymour Butts",
            "Anita Wiener",
            "Dixon Cider",
            "Phil McCracken",
            "Holden Tudix",
            "Jack Goff",
            "Hugh Jorgan",
            "Moe Lester",
            "Ivana Tinkle",
            "Barry McCockiner",
            "Craven Moorehead",
            "Eileen Dover",
            "Pat Myback",
            "Amanda Hugginkiss",
            "Oliver Klozoff",
            "Harry Azcrac",
            "Dwayne Pipe",
            "Hugh Jazz",
            "Ben Chover",
            "Alotta Fagina",
            "Tess Tickle",
            "Suk Mahdik",
            "Sal Monella",
            "Dustin Beaver",
            "Rusty Kuntz",
            "Drew P. Wiener",
            "Buster Cherry",
            "Moe Ron",
            "Lou Sass",
            "Ivana B. Thicc",
            "Hugh G. Rection",
            "Willie Stroker",
            "Jack Pott",
            "Barry D. Hatchet",
            "Manny Faces",
            "Wayne Kerr",
            "Peter Enis",
            "Schwanzus Longus",
            "Kobi",            
            "Sum Ting Wong",
            "Ho Lee Fuk",
            "Wi Tu Low",
            "Ding Bum Ow",
            "Bang Yu Head",
            "Brunhilde Bums",
            "Hildegard Hupf",
            "Waltraud Wackel",
            "Siegfried Schnarch",
            "Friedrich Fummel",
            "Gertrud Grummel",
            "Adelheid Absurd",
            "Günther Gaga",
            "Kunigunde Kicher",
            "Hannelore Hoden"
        };

        public static String GetRandomEntry()
        {
            return botNames.GetRandomEntry();
        }
    }
}

