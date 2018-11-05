﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZScreamMagic
{
    public partial class TilesViewer : Form
    {
        Rom rom;

        OverworldMap map;
        public TilesViewer(Rom rom)
        {
            InitializeComponent();
            this.rom = rom;
        }
        //What do we need for a tile viewer?
        //Load Tile8 Gfx
        //Load Tile16 Data
        //Load Tile32 Data?
        Bitmap b = new Bitmap(128, 7136, PixelFormat.Format4bppIndexed);
        private void TilesViewer_Load(object sender, EventArgs e)
        {
            Tile16[] tiles16 = TilesLoader.LoadTile16(rom);

            //Need to load map data 
            map = new OverworldMap(rom, 0);
            //allgfx16Ptr
            unsafe
            {
                byte* newPdata = (byte*)ZGraphics.allgfx16Ptr.ToPointer();
                
                BitmapData bdata = b.LockBits(new Rectangle(0, 0, 128, 256), ImageLockMode.ReadWrite, PixelFormat.Format4bppIndexed);
                byte* pdata = (byte*)bdata.Scan0;
                int sheetPos = 0;
                for (int i = 0; i < 8; i++)
                {
                    int d = 0;
                    while (d < 2048)
                    {
                        byte mapByte = newPdata[d + (map.blocksets[i] * 2048)];
                        switch (i)
                        {
                            case 0:
                            case 3:
                            case 4:
                            case 5:
                                mapByte += 0x88;
                                break;
                        }
                        
                        pdata[d + (sheetPos * 2048)] = mapByte;
                        d++;
                    }
                    sheetPos++;
                }
                b.UnlockBits(bdata);
                ColorPalette cp = b.Palette;
                for (int i = 0; i < 16; i++)
                {
                    cp.Entries[i] = map.palettes[i+32];
                }
                b.Palette = cp;
            }


            mainTilesDisplay.Image = b;
            //mainTilesDisplay.Refresh();
            //Need to load Palette Map Data

            //Need to load Blockset Map Data

            //-> will be able to draw a tile at this point



        }

        private void mainTilesDisplay_Paint(object sender, PaintEventArgs e)
        {
            /*for(int i = 0;i< 8; i++)
            {
                e.Graphics.DrawImage(b,new Rectangle(0,32*i,128,32),new Rectangle(0,map.blocksets[i]*32,128,32),GraphicsUnit.Pixel);
            }*/

            /*int y = 0;
            int x = 0;
            for (int i = 0;i<256;i++)
            {
                e.Graphics.FillRectangle(new SolidBrush(map.palettes[i]), new Rectangle(x*16, y*16, 16, 16));
                x++;
                if (x >= 16)
                {
                    x = 0;
                    y++;
                }
            }*/

            //overworldPaletteGroup1
            //drawMainPalette(e.Graphics, 0);
            //drawAux1Palette(e.Graphics, 4);
            //drawAux2Palette(e.Graphics, 2);
            //drawAnimatedPalette(e.Graphics, 7);

        }


        //============================================================================
        //DEBUGING CODE
        //============================================================================

 


    }
}
