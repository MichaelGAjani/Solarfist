// FileInfo
// File:"SoundHelper.cs" 
// Solution:"Solarfist"
// Project:"DotNET Framework Helper" 
// Create:"2019-10-10"
// Author:"Michael G"
// https://github.com/MichaelGAjani/Solarfist
//
// License:GNU General Public License v3.0
// 
// Version:"1.0"
// Function:Sound
// 1.Play(bool is_looping=false)
// 2.Stop
//
// File Lines:38
using System.Media;

namespace Jund.NETHelper.MultiMediaHelper
{
    public class SoundHelper
    {
        SoundPlayer player = new SoundPlayer();

        public SoundHelper(string file)
        {
            player.SoundLocation = file;
        }
        public void Play(bool is_looping=false)
        {
            player.LoadAsync();
            if (is_looping) player.Play(); else player.PlayLooping();
        }

        public void Stop() => player.Stop();
    }
}
