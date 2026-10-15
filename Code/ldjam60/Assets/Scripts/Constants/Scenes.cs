using System;
using System.Collections.Generic;
using System.Linq;

using GameFrame.Core;

namespace Assets.Scripts.Constants
{
    public static class Scenes
    {
        //public const String MainMenuName = "MainMenu";
        //private static Scene mainMenu;
        //public static Scene MainMenu
        //{
        //    get
        //    {
        //        if (mainMenu == default)
        //        {
        //            mainMenu = new Scene()
        //            {
        //                Name = MainMenuName,
        //                IsStoppingBackgroundMusic = true
        //            };
        //        }

        //        return mainMenu;
        //    }
        //}

        //public const String GameName = "Game";
        //private static Scene game;
        //public static Scene Game
        //{
        //    get
        //    {
        //        if (game == default)
        //        {
        //            game = new Scene()
        //            {
        //                Name = GameName,
        //                IsStoppingBackgroundMusic = true,
        //                BackgroundClips = new List<String>() { }
        //            };
        //        }

        //        return game;
        //    }
        //}

        //public const String StartName = "Start";
        //private static Scene start;
        //public static Scene Start
        //{
        //    get
        //    {
        //        if (start == default)
        //        {
        //            start = new Scene()
        //            {
        //                Name = StartName,
        //                IsStoppingBackgroundMusic = true,
        //                //AmbienceClips = new List<String>()
        //                //{
        //                //    "WoodSound"
        //                //},
        //                BackgroundClips = new List<String>() { }
        //            };
        //        }

        //        return start;
        //    }
        //}

        //public const String AudioTestName = "AudioTests";
        //private static Scene audioTest;
        //public static Scene AudioTest
        //{
        //    get
        //    {
        //        if (audioTest == default)
        //        {
        //            audioTest = new Scene()
        //            {
        //                Name = AudioTestName,
        //                BackgroundClips = new List<String>()
        //                {
        //                }
        //            };
        //        }

        //        return audioTest;
        //    }
        //}

        public static List<Scene> GetGameScenes()
        {
            return new List<Scene>()
            {
                //Start,
                //MainMenu,
                //Game
            };
        }

        public static List<Scene> GetDevelopmentScenes()
        {
            return new List<Scene>()
            {
                //AudioTest
            };
        }

        public static IList<Scene> GetAll()
        {
            return GetGameScenes().Concat(GetDevelopmentScenes()).ToList();
        }
    }
}
