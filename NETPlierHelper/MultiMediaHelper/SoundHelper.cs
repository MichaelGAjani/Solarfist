using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

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
