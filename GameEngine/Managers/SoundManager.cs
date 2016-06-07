using System.Collections.Generic;
using GameEngine.Engine;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace GameEngine.Managers
{
    /// <summary>
    /// Thread safe singleton without using locks
    /// See link: "http://csharpindepth.com/Articles/General/Singleton.aspx#nested-cctor"
    /// </summary>
    public sealed class SoundManager
    {
        private static readonly SoundManager instance = new SoundManager();
        private Dictionary<string, SoundEffect> SoundPool = new Dictionary<string, SoundEffect>();
        private Dictionary<string, Song> SongPool = new Dictionary<string, Song>();
        private Dictionary<string, SoundCategory> soundCategories = new Dictionary<string,SoundCategory>();
        private Dictionary<string, SoundEffectInstance> soundPoolInstance = new Dictionary<string, SoundEffectInstance>();

        private float masterVolume = 1.0F;

        static SoundManager() { }

        private SoundManager() { }

        /// <summary>
        /// This creates the SoundManager singleton
        /// </summary>
        public static SoundManager Instance
        {
            get
            {
                instance.masterVolume = 1.0F;
                return instance;
            }
        }

        /// <summary>
        /// This method sets the master volume.
        /// Monogame uses range 0 to 1
        /// </summary>
        /// <param name="masterVolume"></param>
        public void SetMasterVolume(float masterVolume)
        {
            if (masterVolume <= 0)
            {
                masterVolume = 0;
            }
            if (masterVolume >= 1.0)
            {
                masterVolume = 1.0F;
            }
            this.masterVolume = masterVolume;
        }

        /// <summary>
        /// This method adds a song to the song pool
        /// </summary>
        /// <param name="songName"></param>
        /// <param name="song"></param>
        public void AddSong(string songName,Song song)
        {
            if(song == null)
            {
                ErrorLogger.Instance.LogErrorToDisk("Error: The song you passed has no data (null), can't add it to the song pool: " + songName, "SoundManagerLog.txt");
                return;
            }

            if(SongPool.ContainsKey(songName))
            {
                ErrorLogger.Instance.LogErrorToDisk("Warning:there already exist a song with the name:" + songName, "SoundManagerLog.txt");
                return;
            }
            SongPool.Add(songName, song);
        }

        /// <summary>
        /// This method removes a song from the song pool
        /// </summary>
        /// <param name="songName"></param>
        public void RemoveSong(string songName)
        {
            if (!SongPool.ContainsKey(songName))
            {
                ErrorLogger.Instance.LogErrorToDisk("Warning:The song you tried to remove is not in the song pool:" + songName, "SoundManagerLog.txt");
                return;
            }
            SongPool.Remove(songName);
        }
        /// <summary>
        /// This method plays the song by the given songName
        /// </summary>
        /// <param name="songName"></param>
        public void PlaySong(string songName)
        {
            if(!SongPool.ContainsKey(songName))
            {
                ErrorLogger.Instance.LogErrorToDisk("Warning:The song you tried to play does not exist in the song pool:" + songName, "SoundManagerLog.txt");
                return;
            }
            MediaPlayer.Play(SongPool[songName]);
        }

        /// <summary>
        /// This method stops the current song from playing
        /// </summary>
        /// <param name="songName"></param>
        public void StopSong()
        {
            MediaPlayer.Stop();
        }

        /// <summary>
        /// This method sets the music volume
        /// </summary>
        /// <param name="volume"></param>
        public void SetMusicVolume(float volume)
        {
            MediaPlayer.Volume = volume * masterVolume;
        }

        /// <summary>
        /// This method returns the current music volume
        /// </summary>
        /// <returns></returns>
        public float GetMusicVolume()
        {
            return MediaPlayer.Volume;
        }

        /// <summary>
        /// This method returns the current master volume setting
        /// </summary>
        public double GetMasterVolume()
        {
            return masterVolume;
        }

        /// <summary>
        /// This method adds a sound effect to the sound pool & creates a sound effect instance for the sound, 
        /// </summary>
        /// <param name="soundName"></param>
        /// <param name="soundEffect"></param>
        public void AddSoundEffect(string soundName, SoundEffect soundEffect)
        {
            //make sure the sound exist in memory
            if(soundEffect == null)
            {
                ErrorLogger.Instance.LogErrorToDisk("Error: The sound effect has no data (null), can't add it to the song pool: " + soundName +"\n", "SoundManagerLog.txt");
                return;
            }

            //If a sound with the given soundName already exist in the pool, log it as a warning
            if (SoundPool.ContainsKey(soundName))
            {
                ErrorLogger.Instance.LogErrorToDisk("Warning:There already exist a sound with the name:" + soundName + "\n", "SoundManagerLog.txt");
                return;
            }

            if (soundPoolInstance.ContainsKey(soundName))
            {
                //this line should never happen, since the two pools always add the same sound
                ErrorLogger.Instance.LogErrorToDisk("Warning:There already exist a sound with the name:" + soundName + "\n", "SoundManagerLog.txt");
                return;
            }

            //add the sound to the soundPool
            SoundPool.Add(soundName,soundEffect);

            //create an instance of the soundEffect
            SoundEffectInstance soundEffectInstance = SoundPool[soundName].CreateInstance();

            //add it to the soundPoolInstance
            soundPoolInstance.Add(soundName, soundEffectInstance);

            //set the sound effect volume to maximum as default
            soundPoolInstance[soundName].Volume = 1.0F;
        }

        /// <summary>
        /// This method removes the given sound from the sound pool & soundPoolInstance
        /// </summary>
        /// <param name="soundname"></param>
        public void RemoveSoundEffect(string soundname)
        {
            if (soundPoolInstance.ContainsKey(soundname))
            {
                soundPoolInstance.Remove(soundname);
            }

            if (SoundPool.ContainsKey(soundname))
            {
                SoundPool.Remove(soundname);
            }
        }

        /// <summary>
        /// This method adds a sound to the given category.
        /// If the category does not exist, it is created 
        /// and the soundname added to it.
        /// </summary>
        /// <param name="soundName"></param>
        /// <param name="category"></param>
        public void AddSoundToSoundCategory(string soundName, string category)
        {
            //First we make sure the sound exist in the sound pool
            if (!SoundPool.ContainsKey(soundName))
            {
                ErrorLogger.Instance.LogErrorToDisk("Error: The sound:" + soundName + " does not exist in the soundPool, can't add a sound that doesn't exist to a sound category\n","SoundManagerLog.txt");
                return;
            }
            //If there is no category with the given name
            if (!soundCategories.ContainsKey(category))
            {
                //create it
                soundCategories.Add(category, new SoundCategory());
            }
            //add the sound to the category
            soundCategories[category].addSound(soundName);
        }

        /// <summary>
        /// This method removes the given sound in the given category
        /// </summary>
        /// <param name="soundName"></param>
        /// <param name="category"></param>
        public void RemoveSoundFromCategory(string soundName,string category)
        {
            if (!soundCategories.ContainsKey(category))
            {
                ErrorLogger.Instance.LogErrorToDisk("Error: The sound category:" + category + " does not exist, can't remove a sound from a category that doesn't exist.\n", "SoundManagerLog.txt");
            }
            soundCategories[category].removeSound(soundName);
        }

        /// <summary>
        /// This method plays the sound with the given name
        /// </summary>
        /// <param name="soundName"></param>
        public void PlaySound(string soundName)
        {
            if (!soundPoolInstance.ContainsKey(soundName))
            {
                ErrorLogger.Instance.LogErrorToDisk("Error: The sound:" + soundName + " does not exist in the sound pool.\n", "SoundManagerLog.txt");
                return;
            }
            soundPoolInstance[soundName].Play();
        }

        /// <summary>
        /// This method sets the volume for the given sound category
        /// </summary>
        /// <param name="volume"></param>
        /// <param name="category"></param>
        public void SetSoundCategoryVolume(float volume, string category)
        {
            List<string> SoundsInCategory;
                        
            if (!soundCategories.ContainsKey(category))
            {
                ErrorLogger.Instance.LogErrorToDisk("Error: The sound category:" + category + " does not exist, can't set volume for a category that doesn't exist\n", "SoundManagerLog.txt");
                return;
            }
            
            SoundsInCategory = soundCategories[category].getListOfSounds();
            
            foreach (string s in SoundsInCategory)
            {
                if (s == null)
                {
                    return;
                }
                soundPoolInstance[s].Volume = masterVolume * volume;
            }

            soundCategories[category].setSoundVolume(volume);
        }

        /// <summary>
        /// Get the volume of a specified sound category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public float GetCategorySoundVolume(string category)
        {
            if(!soundCategories.ContainsKey(category))
            {
                ErrorLogger.Instance.LogErrorToDisk("Error: The sound category:" + category + " does not exist, can't get volume for a category that doesn't exist\n", "SoundManagerLog.txt");
                return 0.0f;
            }

            return soundCategories[category].getSoundVolume();            
        }

        /// <summary>
        /// A inner class holding soundCategories, with a list of sounds belonging to it
        /// </summary>
        private class SoundCategory
        {
            private List<string> sounds;
            private float volume;
            /// <summary>
            /// Constructor for a sound category, creates the list holding the sounds
            /// </summary>
            /// <param name="name"></param>
            public SoundCategory()
            {
            }

            /// <summary>
            /// This method adds a sound to the sound category
            /// </summary>
            /// <param name="name"></param>
            public void addSound(string name)
            {
                if (sounds == null)
                {
                    sounds = new List<string>(); 
                }
                sounds.Add(name);
            }

            /// <summary>
            /// This method removes a sound from the category
            /// </summary>
            /// <param name="name"></param>
            public void removeSound(string name)
            {
                //Make sure the sound exist in the sound pool
                if (!sounds.Contains(name))
                {
                    ErrorLogger.Instance.LogErrorToDisk("Warning: The sound" + name + " does not exist in the category, can't remove a sound that doesn't exist.\n", "SoundManagerLog.txt");
                    return;
                }
                sounds.Remove(name);
            }

            /// <summary>
            /// This method sets the sound volume for the sounds in the category
            /// Monogame uses range 0 to 1
            /// </summary>
            /// <param name="volume"></param>
            public void setSoundVolume(float volume)
            {
                if (volume <= 0)
                {
                    volume = 0;
                }
                if (volume >= 1.0)
                {
                    volume = 1.0F;
                }
                this.volume = volume;
            }

            /// <summary>
            /// This method returns the volume for the sounds in the category
            /// </summary>
            /// <returns></returns>
            public float getSoundVolume()
            {
                return volume;
            }

            /// <summary>
            /// This method returns a string of all the sounds in the category
            /// </summary>
            /// <returns></returns>
            public List<string> getListOfSounds()
            {
                return sounds;
            }
        }
    }
}

