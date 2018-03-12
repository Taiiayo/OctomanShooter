//#define My_Debug

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Octoman_Shooter.Properties;

namespace Octoman_Shooter
{
    public partial class OctomanShooter : Form
    {
        private Ranks rank = Ranks.NOVICE;

        private double previousScore = 0;

        private bool isGameStarted = false;
        private bool isSoundMuted = false;

        private int frameNum = 8;
        private const int splatNum = 3;
        private bool splat = false;

        private int _gameFrame = 0;
        private int _splatTime = 0;

        private int _hits = 0;
        private int _misses = 0;
        private int _totalShoths = 0;
        private double _averageHits = 0;

#if My_Debug
        private int _cursX = 0;
        private int _cursY = 0;
#endif

        private COctoman octoman;
        private CSign _sign;
        private CSplat _splat;
        private CScoreFrame _scoreFrame;
        private CWords theWords;
        private CSound sound;
        private CNoSound noSound;

        Random rnd = new Random();

        public OctomanShooter()
        {
            InitializeComponent();

            Bitmap b = new Bitmap(Resources.aim);
            this.Cursor = CustomCursor.CreateCursor(b, b.Height / 2, b.Width / 2);

            _scoreFrame = new CScoreFrame() { left = 600, top = 10 };
            _sign = new CSign() { left = 10, top = 10 };
            _splat = new CSplat();
            octoman = new COctoman() { left = 10, top = 318 };
            theWords = new CWords() { left = 180, top = 180 };
            sound = new CSound() { left = 900, top = 13 };
            noSound = new CNoSound() { left = 900, top = 13 };

            string fromFile = File.ReadAllText(@"textboxFile.txt");
            previousScore = double.Parse(fromFile);
        }

        private void timerGameLoop_Tick(object sender, EventArgs e)
        {
            if (_gameFrame >= frameNum)
            {
                UpdateOctoman();
                _gameFrame = 0;
            }
            _gameFrame++;

            if (splat)
            {
                if (_splatTime >= splatNum)
                {
                    splat = false;
                    _splatTime = 0;
                    UpdateOctoman();
                }
                _splatTime++;
            }

            this.Refresh();
        }

        private void UpdateOctoman()
        {
            octoman.Update(
                rnd.Next(Resources.octoman.Width / 4, this.Width - Resources.octoman.Width),
                rnd.Next(this.Height / 2, this.Height - (Resources.octoman.Height * 2 - 60))
                );
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics dc = e.Graphics;

            if (splat == true)
            {
                _splat.DrawImage(dc);
            }
            else
            {
                octoman.DrawImage(dc);
            }

            if (isGameStarted == false)
            {
                theWords.DrawImage(dc);
            }

            if (isSoundMuted == true)
            {
                noSound.DrawImage(dc);
            }
            else
            {
                sound.DrawImage(dc);
            }

            _sign.DrawImage(dc);
            _scoreFrame.DrawImage(dc);

#if My_Debug
            TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.EndEllipsis;
            Font _font = new System.Drawing.Font("Stencil", 11, FontStyle.Regular);
            TextRenderer.DrawText(dc, "x = " + _cursX.ToString() + ":" + "y = " + _cursY.ToString(),
                _font, new Rectangle(30, 28, 120, 20), SystemColors.ControlText, flags);
#endif

            // Put scores on the screen
            TextFormatFlags flags = TextFormatFlags.Left;
            Font _font = new System.Drawing.Font("Showcard Gothic", 16, FontStyle.Regular);
            TextRenderer.DrawText(e.Graphics, _totalShoths.ToString(), _font, new Rectangle(793, 50, 200, 50), Color.DarkRed, flags);
            TextRenderer.DrawText(e.Graphics, _hits.ToString(), _font, new Rectangle(793, 86, 200, 50), Color.DarkRed, flags);
            TextRenderer.DrawText(e.Graphics, _misses.ToString(), _font, new Rectangle(793, 123, 200, 50), Color.DarkRed, flags);
            TextRenderer.DrawText(e.Graphics, _averageHits.ToString("F0") + "%", _font, new Rectangle(793, 160, 200, 50), Color.DarkRed, flags);
            TextRenderer.DrawText(e.Graphics, rank.ToString(), _font, new Rectangle(770, 196, 200, 50), Color.DarkRed, flags);



            base.OnPaint(e);
        }

        private void OctomanShooter_MouseMove(object sender, MouseEventArgs e)
        {
#if My_Debug
            _cursX = e.X;
            _cursY = e.Y;
#endif

            this.Refresh();
        }

        private void OctomanShooter_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.X > 120 && e.X < 214 && e.Y > 55 && e.Y < 85) // Start
            {
                isGameStarted = true;
                timerGameLoop.Start();
            }
            else if (e.X > 122 && e.X < 208 && e.Y > 91 && e.Y < 117) // Pause
            {
                isGameStarted = false;
                timerGameLoop.Stop();
            }
            else if (e.X > 114 && e.X < 218 && e.Y > 125 && e.Y < 150) // Reset
            {
                isGameStarted = false;
                _hits = 0;
                _misses = 0;
                _totalShoths = 0;
                _averageHits = 0;

                _gameFrame = 0;
                rank = Ranks.NOVICE;

                timerGameLoop.Stop();
            }
            else if (e.X > 140 && e.X < 191 && e.Y > 159 && e.Y < 184) // Quit
            {
                if (File.Exists(@"textboxFile.txt"))
                {
                    File.WriteAllText(@"textboxFile.txt", _averageHits.ToString("F0"));
                }

                MessageBox.Show("Your current score is: " + _averageHits.ToString("F0") + "%" + " The previous score is: " + previousScore);

                Application.Exit();
            }
            else if (e.X > 897 && e.X < 935 && e.Y > 13 && e.Y < 43) // Sound
            {
                if (isSoundMuted == false)
                {
                    isSoundMuted = true;
                }
                else
                {
                    isSoundMuted = false;
                    noSound.left = sound.left;
                    noSound.top = sound.top;
                }
            }
            else
            {
                if (isGameStarted == false)
                {
                    return;
                }
                if (octoman.Hit(e.X, e.Y))
                {
                    splat = true;
                    _splat.left = octoman.left + 40;
                    _splat.top = octoman.top;

                    _hits++;

                }
                else
                {
                    _misses++;
                }

                _totalShoths = _hits + _misses;
                _averageHits = (double)_hits / (double)_totalShoths * 100.0;

                // set the rank and his speed there

                if (_averageHits < 33)
                {
                    rank = Ranks.NOVICE;
                    frameNum = 8;
                }
                else if (_averageHits >= 33 && _averageHits < 50)
                {
                    rank = Ranks.NOT_BAD;
                    frameNum = 7;
                }
                else if (_averageHits >= 50 && _averageHits < 70)
                {
                    rank = Ranks.MIDMAN;
                    frameNum = 6;
                }
                else if (_averageHits >= 70 && _averageHits < 100)
                {
                    rank = Ranks.WARRIOR;
                    frameNum = 5;
                }
                else
                {
                    rank = Ranks.SAVIOR;
                    frameNum = 4;
                }
            }

            if (isSoundMuted == false)
            {
                FireGun();
            }
        }

        private void FireGun()
        {
            // Fire off the shotgun

            SoundPlayer simpleSound = new SoundPlayer(Resources.gunshot);
            simpleSound.Play();
        }
    }
}
