using System;
using EP.Ner;
using EP.Ner.Core;
using EP.Morph;
using System.Diagnostics;
using System.Text;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = Stopwatch.StartNew();

            // инициализация - необходимо проводить один раз до обработки текстов
            Console.Write("Initializing ... ");

            ProcessorService.Initialize(MorphLang.RU | MorphLang.EN);
            // инициализируются все используемые анализаторы
            EP.Ner.Money.MoneyAnalyzer.Initialize();
            EP.Ner.Uri.UriAnalyzer.Initialize();
            EP.Ner.Phone.PhoneAnalyzer.Initialize();
            EP.Ner.Definition.DefinitionAnalyzer.Initialize();
            EP.Ner.Date.DateAnalyzer.Initialize();
            EP.Ner.Bank.BankAnalyzer.Initialize();
            EP.Ner.Geo.GeoAnalyzer.Initialize();
            EP.Ner.Address.AddressAnalyzer.Initialize();
            EP.Ner.Org.OrganizationAnalyzer.Initialize();
            EP.Ner.Person.PersonAnalyzer.Initialize();
            EP.Ner.Mail.MailAnalyzer.Initialize();
            EP.Ner.Transport.TransportAnalyzer.Initialize();
            EP.Ner.Decree.DecreeAnalyzer.Initialize();
            EP.Ner.Titlepage.TitlePageAnalyzer.Initialize();
            EP.Ner.Booklink.BookLinkAnalyzer.Initialize();
            EP.Ner.Named.NamedEntityAnalyzer.Initialize();

            sw.Stop();
            Console.WriteLine("OK (by {0} ms), version {1}", (int)sw.ElapsedMilliseconds, ProcessorService.Version);

            // анализируемый текст
            string txt = "Единственным конкурентом «Трансмаша» на этом дебильном тендере было ООО «Плассер Алека Рейл Сервис», основным владельцем которого является австрийская компания «СТЦ-Холдинг ГМБХ». До конца 2011 г. эта же фирма была совладельцем «Трансмаша» вместе с «Тако» Краснова. Зато совладельцем «Плассера», также до конца 2011 г., был тот самый Карл Контрус, который имеет четверть акций «Трансмаша». ";

            // создаём экземпляр обычного процессора
            using (Processor proc = ProcessorService.CreateProcessor())
            {
                // анализируем текст
                AnalysisResult ar = proc.Process(new SourceOfAnalysis(txt));

                // результирующие сущности
                Console.WriteLine("Entities: ");
                foreach (var e in ar.Entities)
                    Console.WriteLine(e);

                // пример выделения именных групп
                Console.WriteLine("Noun groups: ");
                for (Token t = ar.FirstToken; t != null; t = t.Next)
                {
                    if (t.GetReferent() != null) continue; // токены с сущностями игнорируем
                    // пробуем создать именную группу
                    NounPhraseToken npt = NounPhraseHelper.TryParse(t, NounPhraseParseAttr.AdjectiveCanBeLast);
                    if (npt == null) continue; // не получилось
                    Console.WriteLine(npt);
                    t = npt.EndToken; // указатель на последний токен группы
                }
            }

            Console.WriteLine("Over!");
        }
    }
}
