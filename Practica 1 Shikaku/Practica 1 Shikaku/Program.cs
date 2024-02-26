//name
//name
namespace Practica_1_Shikaku
{
    internal class Program
    {
        const bool DEBUG = true;
        

        struct Coor
        { // coordenadas en el tablero 
            public int x, y;
        }

        struct Pilar
        { // pilar en el tablero
            public Coor coor; // posición en el tablero
            public int val;
        } // valor

        struct Rect
        {  // rectangulo determinado por dos esquinas
            public Coor lt, rb;
        }  // left-top, right-bottom

        struct Tablero
        { // tamaño, pilares, rectángulos marcados
            public int fils, cols; // num fils y cols del tablero   
            public Pilar[] pils;  // array de pilares
            public Rect[] rects;  // array de rectángulos
            public int numRects;
        } // num de rectángulos definidos = prim pos libre en rect



        static void Main()
        {
            Coor act, ori;
            act.x = 0;
            act.y = 0;
            ori.x = -1;
            ori.y = -1;

            bool antc = false;


            


            //Lectura archivo
            LeeNivel("puzzles/000.txt", out Tablero tab);
            Render(tab, act, ori);

            while (true)
            {
                char ch = leeInput();
                if (ch != ' ')
                {
                    ProcesaInput(ch, ref tab, ref act, ref ori, ref antc);
                    
                    Render(tab, act, ori);
                }

            }
            Console.WriteLine();
            Console.WriteLine();


        }




        static char leeInput()
        {
            char d = ' ';
            while (d == ' ')
            {
                if (Console.KeyAvailable)
                {
                    string tecla = Console.ReadKey().Key.ToString();
                    switch (tecla)
                    {
                        case "LeftArrow":
                            d = 'l';
                            break;
                        case "UpArrow":
                            d = 'u';
                            break;
                        case "RightArrow":
                            d = 'r';
                            break;
                        case "DownArrow":
                            d = 'd';
                            break;
                        case "Spacebar":
                            d = 'c';
                            break;
                        case "Escape":
                        case "Q":
                            d = 'q';
                            break;
                    }
                }
            }
            return d;
        }

        static void LeeNivel(string file, out Tablero tab)
        {
            StreamReader entrada;
            entrada = new StreamReader(file);

            //Coordenadas
            tab.fils = int.Parse(entrada.ReadLine());
            tab.cols = int.Parse(entrada.ReadLine());

            //Array pilares
            tab.pils = new Pilar[tab.fils * tab.cols];

            int jPils = 0;
            int ycord = 0;
            while (!entrada.EndOfStream)
            {
                string linea = new string(entrada.ReadLine());
                string[] num = linea.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < num.Length; i++)
                {
                    if (num[i] != "-")
                    {
                        tab.pils[jPils].coor.x = i;
                        tab.pils[jPils].coor.y = ycord;
                        tab.pils[jPils].val = int.Parse(num[i]);
                        jPils++;
                    }
                }
                ycord++;
            }

            //Array Rectángulos
            tab.rects = new Rect[tab.cols * tab.fils];

            //numRects
            tab.numRects = 0;

            //cierre
            entrada.Close();

        }

        static Rect NormalizaRect(Coor c1, Coor c2)
        {
            Rect aux1 = new Rect();


            int aux = 0;

            if (c1.x > c2.x)
            {
                aux = c1.x;
                c1.x = c2.x;
                c2.x = aux;
            }


            if (c1.y > c2.y)
            {
                aux = c1.y;
                c1.y = c2.y;
                c2.y = aux;
            }

            aux1.lt = c1;
            aux1.rb = c2;

            return aux1;

        }

        static void Render(Tablero tab, Coor act, Coor ori)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.White;
            for (int j = 0; j < tab.fils + 1; j++)
            {
                for (int i = 0; i < tab.cols + 1; i++)
                {
                    Console.Write("+" + "   ");
                }
                Console.WriteLine();
                Console.WriteLine();

            }

            //Pilares:
            int x, y;
            int k = 0;
            while (k < tab.pils.Length && tab.pils[k].val != 0)
            {
                //Transformar coordenadas:
                x = 4 * tab.pils[k].coor.x + 2;
                y = 2 * tab.pils[k].coor.y + 1;

                //Pintar pilares:
                Console.SetCursorPosition(x, y);
                Console.Write(tab.pils[k].val);
                k++;
            }

            //rectángulos ya marcados
            for (int i = 0; i < tab.numRects; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                RenderRect(tab.rects[i]);
            }

            //rectángulo en proceso

            Rect aux = NormalizaRect(act, ori);

            if (ori.x >= 0 && ori.y >= 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                RenderRect(aux);
            }


            //Debug

            if (DEBUG)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(0, tab.fils * 2 + 1);
                Console.WriteLine();
                Console.WriteLine("Rects : (" + aux.lt.x + "," + aux.lt.y + ") - (" + aux.rb.x + "," + aux.rb.y + ")");
                Console.Write("Ori: (" + ori.x + "," + ori.y + ")   Act: (" + act.x + "," + act.y + ")");
            }

            //Cursor
            Console.SetCursorPosition(act.x * 4 + 2, act.y * 2 + 1);

        }

        static Coor Transformar(Coor a)
        {
            a.x = 4 * a.x + 2;
            a.y = 2 * a.y + 1;
            return a;
        }

        static void RenderRect(Rect r)
        {
            r.lt = Transformar(r.lt);
            r.rb = Transformar(r.rb);

            // ---
            for (int j = r.lt.x - 1; j < r.rb.x; j = j + 4)
            {
                for (int i = j; i < j + 3; i++)
                {

                    Console.SetCursorPosition(i, r.lt.y - 1);
                    Console.Write("-");
                    Console.SetCursorPosition(i, r.rb.y + 1);
                    Console.Write("-");
                }

            }

            // |
            for (int i = r.lt.y; i < r.rb.y + 1; i = i + 2)
            {
                Console.SetCursorPosition(r.lt.x - 2, i);
                Console.Write("|");
                Console.SetCursorPosition(r.rb.x + 2, i);
                Console.Write("|");
            }

        }

        static void ProcesaInput(char ch, ref Tablero tab, ref Coor act, ref Coor ori, ref bool antc)
        {
            
            
            if (ch == 'l' && act.x > 0)
            {
                act.x--;
            }
            else if (ch == 'r' && act.x < tab.cols - 1)
            {
                act.x++;
            }
            else if (ch == 'u' && act.y > 0)
            {
                act.y--;
            }
            else if (ch == 'd' && act.y < tab.fils - 1)
            {
                act.y++;
            }
            else if (ch == 'c')
            {
                if(!antc)
                {
                    ori = act;
                    EliminaRect(ref tab, ori);
                    antc = true;
                }
                else
                {
                    InsertaRect(ref tab, ori, act);
                    ori.x = -1;
                    ori.y = -1;
                    antc = false;
                }
            }
        }

        
        static bool Dentro(Coor c, Rect r) //método para saber si cierta coordenada está en cierto rectángulo
        {
            if((c.x == r.lt.x || c.x == r.rb.x) && (r.lt.y <= c.y && c.y <= r.rb.y))
            {
                return true;
            }
            return false;

        }
        
        static bool InterSect(Rect r1, Rect r2)
        {
            if((Dentro(r1.lt, r2)) || (Dentro(r1.rb, r2)))
            {
                return true;
            }

            return false;
        }
        
        static void InsertaRect(ref Tablero tab, Coor c1, Coor c2)
        {
            //Normaliza el rectángulo dado
            Rect actual = NormalizaRect(c1, c2);

            if (tab.numRects > 0)
            {
                //búsqueda de intersección array de rectángulos para ver si el nuevo NO intersecta con ninguno
                int i = 0;
                while (i < tab.numRects && !InterSect(actual, tab.rects[i])) i++;

                if (i >= tab.numRects) //no solapa con ninguno de los existentes
                {
                    tab.numRects++;
                    tab.rects[i] = actual;
                }
            }
            else
            {
                tab.numRects++;
                tab.rects[0] = actual;
            }
            
            
        }

        static bool EliminaRect(ref Tablero tab, Coor c)
        {
            if (tab.numRects > 0)
            {
                //búsqueda
                int i = 0;
                while (i < tab.numRects && !Dentro(c, tab.rects[i])) i++;

                if (i >= tab.numRects)
                {
                    return false;
                }
                else
                {
                    //eliminar rectángulo encontrado
                    for (int j = i; j < tab.numRects; j++)
                    {
                        tab.rects[j] = tab.rects[j + 1];
                    }
                    tab.numRects--;
                    return true;
                }
            }
            else
            {
                return false;
            }
                

        }

        /*
         //eliminar rectángulo encontrado
                Rect ant = tab.rects[tab.rects.Length - 1];
                for(int j = tab.rects.Length -1; j >= i; j--)
                {
                    Rect sig = tab.rects[j - 1];
                    tab.rects[j - 1] = ant;
                    ant = sig;
                }
                tab.numRects--;
                return true;

        for(int j = i; j < tab.rects.Length; j++)
                {
                    tab.rects[j] = tab.rects[j + 1];
                }

        (c.x <= r.rb.x && c.y <= r.rb.y) || (c.x >= r.lt.x && c.y >= r.lt.y)

         */

    }
}