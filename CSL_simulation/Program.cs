using System;
// Dodanie przestrzeni nazw z biblioteki CSL
using CSL;
using CSL.Time;
using CSL.Groups;
using CSL.Generators.Discrete;
using CSL.Statistics;

namespace Sim
{

    class Simulation
    {
        Element[] zgloszenie;
        public Random random;
        public void Run()
        {
            uint maxLiczbaZgloszen = 1000;
            uint srOdstepZgloszen = 1000;
            uint srCzasObsl = 450;
            uint sumaZgloszen = 10000;
            zgloszenie = new Element[maxLiczbaZgloszen];

            for (uint i = 0; i < maxLiczbaZgloszen; i++)
            {
                zgloszenie[i] = new Element();
            }
            SetGroup wolne = new SetGroup(maxLiczbaZgloszen);
            QueueGroup kolejka = new QueueGroup(maxLiczbaZgloszen);
            Timer nadZgl = new Timer();
            Timer konObsl = new Timer();
            int stanObsl;

            NegExp genOdstZgl = new NegExp(srOdstepZgloszen);
            NegExp genCzasuObsl = new NegExp(srCzasObsl);

            StatD statCzasuPobytu = new StatD();
            StatCRect statDlKol = new StatCRect();
            uint dlKol = 0;
            Timer zegarStat = new Timer();

            Hist histLP = new Hist(7, 0, 1);

            long liczbaObs;
            long sumaLiczbObs = 10000;

            uint El = uint.MaxValue;
            random = new Random();

            wolne.Load();
            kolejka.Zero();
            zegarStat.t = 0;

            statDlKol.Clear();
            histLP.Clear();
            statCzasuPobytu.Clear();

            liczbaObs = 0;
            stanObsl = -1;
            nadZgl.t = 0;
            nadZgl.SetOn();

            while (true)
            {
                if (nadZgl.Now())
                {
                    wolne.Find(ref El, FindParameters.FIRST);
                    wolne.From(El);
                    zgloszenie[El].LP = 0;
                    zgloszenie[El].t = 0;
                    kolejka.To(El);
                    dlKol++;
                    statDlKol.Add(dlKol, -zegarStat.t);
                    nadZgl.t = genOdstZgl.Get();
                }
                if (konObsl.Now())
                {
                    zgloszenie[stanObsl].LP++;
                    if (!BRet(ref random))

                    { //obsluzono i wychodzi z systemu
                        liczbaObs++;

                        statCzasuPobytu.Add(-zgloszenie[stanObsl].t);
                        histLP.Add(zgloszenie[stanObsl].LP);
                        wolne.To((uint)stanObsl);

                    }
                    else
                    { //obsluzono, ale zwracamy do kolejki
                        kolejka.To((uint)stanObsl);

                        dlKol++;
                        statDlKol.Add(dlKol, -zegarStat.t);

                    }
                    //konczenie obslugi
                    stanObsl = -1;
                    konObsl.SetOff();

                }
                if (kolejka.Find(ref El, FindParameters.FIRST))
                {
                    if (stanObsl == -1)
                    {
                        kolejka.From(El);
                        dlKol--;
                        statDlKol.Add(dlKol, -zegarStat.t);
                        stanObsl = (int)El;
                        konObsl.t = genCzasuObsl.Get();
                        konObsl.SetOn();

                    }
                }
                else
                {
                    konObsl.SetOff();
                }

                if (liczbaObs >= sumaLiczbObs)
                {
                    break;
                }
                Time.TimeFlow();
            }
            double sr = 3.14;
            Console.WriteLine("Wyniki po przejsciu " + liczbaObs + " obserwacji.");
            Console.WriteLine("Czas symulacji: " + -zegarStat.t);
            statCzasuPobytu.GetStat(ref sr);
            Console.WriteLine("Sredni czas pobytu w systemie: " + sr);
            statDlKol.GetStat(ref sr, -zegarStat.t);
            Console.WriteLine("Srednia dlugosc kolejki: " + sr);
            Console.WriteLine("Histogram liczby przejsc: ");
            Console.WriteLine(histLP.Out());
            Console.ReadKey();
        }
        bool BRet(ref Random _random)
        {
            double next = _random.NextDouble() * 3;
            if (next > 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        class Element : Timer
        {
            public uint LP;
        }

        internal class Program
        {
            static void Main(string[] args)
            {
                Simulation simulation = new Simulation();
                simulation.Run();
            }
        }
    }
}




