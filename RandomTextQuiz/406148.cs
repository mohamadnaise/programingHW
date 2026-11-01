using System;
namespace RandomTextQuiz
{
    class Program
    {
          //مولد الارقام العشوائية
        static Random rnd = new Random();

        //مصفوفة ثنائية ابعاد بتحتوي عكلمات الاسئلة
        static string[][] words ;

        //الاجوبة الصحيحة لكل سؤال ، 4 اجوبة بنفس الترتيب ، كلمات فيها احرف صغيرة ، وكلمات فيها محارف خاصة
        static int[][] correctAnswers ;

        //اجوبة المستخدم لكل سؤال  ، 4 اجوبة بنفس الترتيب
        static int[][] userAnswers ;

        //تقييم كل سؤال ، 0 كل الاجوبة خطأ ، 1 كلها صحيحة ، 2مع بعض مزيج
        static int[] scores ;


        //شباب هاد التابع حتى يتفقد الرقم يلي رح يدخلو المستخدم لعدد الاسئلة ، لازم ما يساوي الصفر ويكون اصغر من 9 
        static bool IsPositiveIntegar(string text)
        {
            if( text.Length == 0 || text.Length > 9 ) //هون الشرط انو ما يساوي 0 واصغر من 9
            {
                return false;        
            } else
            {
                for (int i = 0; i < text.Length; i++)//كل محرف لازم يكون رقم بين 0 و 9
                {
                    if( text[i] < '0' || text[i] > '9' )
                    {
                        return false;
                    }
                }
                return int.Parse(text) > 0;//نتاكد انو العدد اكبر من صفر 
            }
        }

         //التابع هاد كرمال نسال المستخدم عن عدد الاسئلة
        static int ReadNumberOfQuestion()
        {
            Console.WriteLine("Please enter the maximum number of questions");
            string text = Console.ReadLine();

            while(!IsPositiveIntegar(text))//تكرار الطلب اذا الرقم غلط
            {
                Console.WriteLine("The number of questions should be an integer > 0 , please enter again");
                text = Console.ReadLine();
            }
            return int.Parse(text);
        }

         //التابع هاد بيتحقق من المحارف 
        static bool IsAcceptedChar(char c)
        {
            bool isUpper = ( c >= 'A' && c <= 'Z');//حرف كبير
            bool isLower = ( c >= 'a' && c <= 'z');//حرف صغير
            bool isDigit = ( c >= '0' && c <= '9');//رقم بيم 0 و 9

            return isUpper || isLower || isDigit ;
        }

        //تابع بيعد المحارف المقبولة بالنص
        static int CountAcceptedChars(string text)
        {
            int count = 0;
            for (int i = 0; i < text.Length ; i++)
            {
                if(IsAcceptedChar(text[i]))
                {
                    count+=1;
                }
            }
            return count;
        }
        
        //تابع هاد بيطلب من اللاعب معلوماته حتى يدخلها  وبحيث تكون مطابقة للشروط يلي حددناها
        static string ReadPlayerInfo()
        {
            string text;
            do
            {
                Console.WriteLine("Please enter your name (first name and last name) and your SVU id number and your hobbies and your favorite color and your favorite food");
                Console.WriteLine(" with a space between each part (Accepted Chars: A-Z a-z 0-9)");
                text = Console.ReadLine();

                if(GetDistinctChars(text).Length < 6)
                {
                    Console.WriteLine("The entered text should contain at least 6 of Accepted chars");
                }
            }
            while (GetDistinctChars(text).Length < 6);
            return text;
        }
        
        //تابع بيطلع المحارف المميزة يلي دخلا اللاعب
        static string GetDistinctChars(string text)
        {
            string result = "";
            for(int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if(IsAcceptedChar(c) && result.IndexOf(c) < 0)
                {
                    result += c;
                }
            }
            return result;
        }

        //التابع هاد بجهز المصفوفات حسب عدد الاسئلة
        static void PrepareArrays(int numberOfQuestions)
        {
            words = new string[numberOfQuestions][];
            correctAnswers = new int[numberOfQuestions][];
            userAnswers = new int[numberOfQuestions][];
            scores = new int[numberOfQuestions];
        }

        //تابع توليد حرف ابجدي عشوائي
        static char GenerateLetter()
        {
            if(rnd.Next(2) == 0)
            {
                return (char)('A' + rnd.Next(26));
            } else
            {
                return (char)('a' + rnd.Next(26));
            }
        }

        //تابع توليد محرف عشوائي حسب الشروط
        static char GenerateAnyChar()
        {
            int kind = rnd.Next(4);
            if(kind == 0)
            {
                return (char)('A' + rnd.Next(26));//حرف كبير A-Z
            } else if(kind == 1)
            {
                return (char)('a' + rnd.Next(26));//حرف صغير a-z
            } else if(kind == 2)
            {
                return (char)('0' + rnd.Next(10));//رقم بين 0 و 9
            } else
            {
                return "?!;#"[rnd.Next(4)];
            }
        }

        // تابع يولد كلمة عشوائية طولها بين 5 و 10 واول محرف هو حرف ابجدي
        static string GenerateWord()
        {
            int length = rnd.Next(5,11);
            string word = "" ;

            for ( int i = 0; i < length; i++)
            {
                if (i==0)
                {
                word += GenerateLetter() ;
                }else
                {
                    word += GenerateAnyChar();
                }
            }
            return word;
        }

        //تابع لتوليد كلمات السؤال من 4 كلمات ل 8
        static string[] GenerateQuestionWord()
        {
            int count = rnd.Next(4,9);
            string[] questionWords = new string[count];

            for( int i = 0; i <count; i++)
            {
                questionWords[i] = GenerateWord();
            }
            return questionWords;
        }

        //تابع بيحسب مجموع اطوال كلمات السؤال
        static int SumOfLengths(string[] questionWords)
        {
            int sum = 0 ;
            for (int i = 0 ; i < questionWords.Length; i++)
            {
                sum += questionWords[i].Length;
            }
            return sum ;
        }

        //تابع بيتحقق اذا الكلمة فيها حرف صوتي واحد على الاقل
        static bool HasVowel(string word)
        {
            for (int i = 0 ; i < word.Length ; i++)
            {
                if("aAeEiIoOuUYy".IndexOf(word[i]) >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        //تابع بيتحقق اذا الكلمة بتحتوي حرف صغير واحد عالاقل
        static bool HasLowerLetter(string word)
        {
            for (int i = 0 ; i < word.Length ; i++)
            {
                if (word[i] >= 'a' && word[i] <= 'z')
                {
                    return true ;
                }
            }
            return false ;
        }

        //تابع بيتحقق اذا في محرف خاص عالاقل بالكلمة
        static bool HasSpecialChar (string word)
        {
            for (int i = 0 ; i < word.Length ; i++)
            {
                if("?!;#".IndexOf(word[i]) >= 0 )
                {
                    return true;
                }
            }
            return false ;
        }

        //تابع بيحسب الاجوبة الصحيحة
        static int[] CalculateCorrectAnswers(string[] questionWords)
        {
            int[] answers = new int[4];
            answers[0] = SumOfLengths(questionWords);//اطوال

            for (int i = 0 ; i < questionWords.Length; i++)
            {
                if(HasVowel(questionWords[i]))//احرف صوتية
                {
                answers[1] += 1 ;
                }
                if(HasLowerLetter(questionWords[i]))//احرف صغيرة
                {
                    answers[2] += 1 ;
                }
                if(HasSpecialChar(questionWords[i]))//احرف خاصة
                {
                    answers[3] += 1 ;
                }
            }
            return answers;
        }

        //تتابع للتحقق من انو لتابع صحيح مو سالب
        static bool IsNonNegativeIntegar(string text)
        {
           if ( text.Length == 0 || text.Length >9)
            {
                return false ;
            }  

            for ( int i = 0 ; i < text.Length ; i++)
            {
                if( text[i] < '0' || text[i] > '9')
                {
                    return false ;
                }
            }
            return true ;
        }

        //تابع بيتحقق من النص هل هو كلمة وبأي حالة احرف
        static bool IsIgnore(string text)
        {
            return text.ToUpper() == "IGNORE" ;
        }

        //تابع بيقرا الجواب وبعيدو حتى يدخل المستخدم عدد صحيح مو سالب
        static int ReadAnswer(string prompt)
        {
            Console.Write(prompt) ;
            string text = Console.ReadLine();

            while ( !IsNonNegativeIntegar(text))
            {
                Console.WriteLine("The answer should be an integer >= 0 , please enter it again :");
                text = Console.ReadLine();
            }
            return int.Parse(text);
        }

        //تابع بيحسب تقييم السؤال ، 0 كلن غلط ، 1 كلن صح ، 2 التنين سوا
        static int CalculateScore(int[] correct, int[] user)
        {
            int rightCount = 0;

            for( int i = 0 ; i < 4 ; i++)
            {
                if( correct[i] == user[i])
                {
                    rightCount += 1 ;
                }
            }
            if ( rightCount == 0)
            {
                return 0;
            }
            else if (rightCount == 4)
            {
                return 1 ;
            }
            else
            {
                return 2 ;
            }
        }

        //التابع هاد بيولد سؤال واحد رقمه q وبيخزن الكلمات والاجوبة والتقييم
        static void AskQuestion(int q)
        {
            //توليد الكلمات وحساب الاجوبة الصح وتخزينهم
            words[q] = GenerateQuestionWord();
            correctAnswers[q] = CalculateCorrectAnswers(words[q]);
            userAnswers[q] = new int[4] ;

            // عرض السؤال
            Console.WriteLine("Question " + ( q + 1 ) + ":");
            for (int i = 0 ; i <words[q].Length ; i++)
            {
                Console.Write(words[q][i] + " " );
            }
            Console.WriteLine();
            Console.WriteLine("write Ignore to skip this question");
            //الجواب الاول بيقبل رقم او كلمة
            Console.Write("Total length of all words: ");
            string text = Console.ReadLine();

            while (!IsIgnore(text) && !IsNonNegativeIntegar(text))
            {
                Console.WriteLine("Please enter an integer >= 0 (or Ignore):");
                text = Console.ReadLine();
            }
            if(IsIgnore(text))
            {
                for(int i=0 ; i < 4 ;i++)//السؤال متجاهل بنخزن -1 بكل الاجوبة وبينحسب غلط
                {
                    userAnswers[q][i] = -1 ;
                }
            }
            else
            {
                //المستخدم بدا يجاوب وغير ممكن يتم التجاهل بعد هلق
                userAnswers[q][0] = int.Parse(text);
                userAnswers[q][1] = ReadAnswer("Number of words containing vowels : ");
                userAnswers[q][2] = ReadAnswer("Number of words containing lowercase letters : ");
                userAnswers[q][3] = ReadAnswer("Number of words containig special chars (? ! # :)");

            }
            scores[q] = CalculateScore(correctAnswers[q],userAnswers[q]);//حساب التقييم وتخزينه
        }

        //
        static void RunQuestions(int numberOfQuestion)
        {
            for (int q=0 ; q < numberOfQuestion ; q++)
            {
                AskQuestion(q);
                Console.WriteLine();
            }
        }

       //التابع هاد بيعرض اجوبة سؤال واحد وبيعرض الي تم تجاهله 
        static void PrintAnswers(int[] answers)
        {
            if( answers[0] == -1)
            {
                Console.WriteLine("Ignored");
            }
            else
            {
                Console.WriteLine(
                    "Total length: " +
                 answers[0] + 
                " | Vowels: " +
                 answers[1] +
                 " | Lowercase: " + 
                answers[2]+
                 " | Special: " +
                 answers[3]
                 );
            }
        }

        //الخيار الاول : عرض نسبة الاجوبة الصح
        static void ShowPercentage()
        {
            int totalAnswers = words.Length * 4;//لكل سؤال اربع اجوبة
            int rightAnswers = 0 ;

            //حساب الاجوبة الصح بكل الاسئلة
            for ( int q = 0 ; q < words.Length ; q++)
            {
                for( int i = 0 ; i < 4 ; i++)
                {
                    if ( correctAnswers[q][i] == userAnswers[q][i])
                    {
                        rightAnswers += 1 ;
                    }
                }
            }
            double percentage = rightAnswers * 100.0 / totalAnswers ;//حساب النسبة المئوية
            double rounded = (int)(percentage * 100.0 +0.5) /100.0 ;//تقريبها الى منزلتين عشريتين
            Console.WriteLine("Correct answers: " + rightAnswers + " out of " + totalAnswers);
            Console.WriteLine("Percentage of correct answers: " + rounded + "%");
        }

        //الخيار2 عرض الكلمات المولدة لكل سؤال
        static void ShowWords()
        {
            for (int q = 0 ; q < words.Length ; q++)
            {
                Console.Write("Question " + (q+1) + ": ");
                for (int i =0 ; i < words[q].Length; i++)
                {
                    Console.Write(words[q][i] + " ");
                }
                Console.WriteLine();
            }
        }

        //الخيار 3 عرض اجوبة المستخدم بدون عرض الكلمات 
        static void ShowUserAnswers()
        {
            for ( int q = 0 ; q < words.Length ; q++)
            {
                Console.Write("Question " + ( q+1 ) + " (your answers): ");
                PrintAnswers(userAnswers[q]);
            }
        }

        //الخيار 4 رعض الاجوبة الصحيحة بدون عرض الكلمات
        static void ShowCorrectAnswers()
        {
            for( int q = 0 ; q < words.Length; q++)
            {
                Console.Write("Question " + ( q+1 ) + " (correct answers): ");
                PrintAnswers(correctAnswers[q]);
            }
        }

        //الخيار 5 بيعرض اجوية المستخدم مع الاجوبة الصحيحة بدون عرض الكلمات
        static void ShowBothAnswers()
        {
            for (int q = 0 ; q < words.Length ; q++)
            {
                Console.WriteLine("Question " + ( q+1 ) + ":");
                Console.Write(" Your answers : ");
                PrintAnswers(userAnswers[q]);
                Console.Write(" correct answers : ");
                PrintAnswers(correctAnswers[q]);
            }
        }

        //التابع هاد بيتحقق اذا النص كلمة حالة احرف
        static bool IsQuit(string text)
        {
            return text.ToUpper() == "QUIT" ;
        }

        //
        static void ShowMenu()
        {
            string choice ;

            do
            {
                Console.WriteLine();
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1 - Show the percentage of correct answers");
                Console.WriteLine("2 - Show the generated words of each question");
                Console.WriteLine("3 - Show your answers (without words)");
                Console.WriteLine("4 - Show the correct answers (without words)");
                Console.WriteLine("5 - Show your answers and the correct answers (without words)");
                Console.WriteLine("Type quit to exit");
                choice = Console.ReadLine();

                if(!IsQuit(choice))
                {
                    if(choice.Length != 1)
                    {
                        Console.WriteLine("Invalid option, please try again");
                    }
                        else
                {
                    switch (choice[0])
                    {
                        case '1':
                            ShowPercentage();
                            break;
                        case '2':
                            ShowWords();
                            break;
                        case '3':
                            ShowUserAnswers();
                            break;
                        case '4':
                            ShowCorrectAnswers();
                            break;
                        case '5':
                            ShowBothAnswers();
                            break;
                        default:
                            Console.WriteLine("Invalid option, please try again.");
                            break;
                       }
                   }
                }
            } 
            while (!IsQuit(choice));//بيتكرر حتى يدخل المستخدم quit
        }




        static void Main(string[] args)
        {
            //اوامر طباعة بعض معلومات الجهاز المنجز عليه الوظيفة 
            Console.WriteLine("Windows version: {0}",Environment.OSVersion);
            Console.WriteLine("64 Bit operating system? : {0}" , Environment.Is64BitOperatingSystem ? "Yes":"No");
            Console.WriteLine("PC Name : {0}", Environment.MachineName);
            Console.WriteLine("Number of CPUS : {0}" , Environment.ProcessorCount);
            Console.WriteLine("Windows folder : {0}" , Environment.SystemDirectory);
            Console.WriteLine("Logical Drives Available : {0}" , String.Join(", ", Environment.GetLogicalDrives()).TrimEnd(',', ' ').Replace("\\", String.Empty));

            int numberOfQuestions = ReadNumberOfQuestion();//استدعاء للتابع يلي بيسأل المستخدم عن عدد الاسئلة 


            string playerInfo = ReadPlayerInfo();//استدعاء للتابع يلي بيطلب المعلوماتامن اللاعب
            Console.WriteLine("Your full name and your SVU id number and your hobbies and your favorite color and your favorite food is : {0}", playerInfo);
            Console.WriteLine("Distinct Chars are : {0}", GetDistinctChars(playerInfo));//استدعاء للتابع يلي بيطلع المحارف المميزة يلي دخلا اللاعب

            PrepareArrays(numberOfQuestions);
            RunQuestions(numberOfQuestions);
            ShowMenu();

        }    
    }
}
