using System;
using System.Collections.Generic;
using System.Linq;

using GameFrame.Core;

namespace Assets.Scripts.Constants
{
    public static class Scenes
    {
        public const String MainMenuName = "MainMenu";
        private static Scene mainMenu;
        public static Scene MainMenu
        {
            get
            {
                if (mainMenu == default)
                {
                    mainMenu = new Scene()
                    {
                        Name = MainMenuName,
                        IsStoppingBackgroundMusic = true
                    };
                }

                return mainMenu;
            }
        }

        public const String GameName = "Game";
        private static Scene game;
        public static Scene Game
        {
            get
            {
                if (game == default)
                {
                    game = new Scene()
                    {
                        Name = GameName,
                        IsStoppingBackgroundMusic = true,
                        BackgroundClips = new List<String>() { }
                    };
                }

                return game;
            }
        }


        public const String CreditsName = "Credits";
        private static Scene credits;
        public static Scene Credits
        {
            get
            {
                if (credits == default)
                {
                    credits = new Scene()
                    {
                        Name = CreditsName,
                    };
                }

                return credits;
            }
        }

        public const String PhysicsTestName = "PhysicsTest";
        private static Scene physicsTest;
        public static Scene PhysicsTest
        {
            get
            {
                if (physicsTest == default)
                {
                    physicsTest = new Scene()
                    {
                        Name = PhysicsTestName,
                    };
                }

                return physicsTest;
            }
        }

        public const String MapTestName = "MapTest";
        private static Scene mapTest;
        public static Scene MapTest
        {
            get
            {
                if (mapTest == default)
                {
                    mapTest = new Scene()
                    {
                        Name = MapTestName,
                    };
                }

                return mapTest;
            }
        }


        public static List<Scene> GetGameScenes()
        {
            return new List<Scene>()
            {
                MainMenu,
                Game,
                Credits,
            };
        }

        public static List<Scene> GetDevelopmentScenes()
        {
            return new List<Scene>()
            {
                PhysicsTest, 
                MapTest,
            };
        }

        public static IList<Scene> GetAll()
        {
            return GetGameScenes().Concat(GetDevelopmentScenes()).ToList();
        }
    }
}
