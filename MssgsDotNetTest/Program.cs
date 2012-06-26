using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MssgsDotNet;
using MssgsDotNet.Commands;
using MssgsDotNet.Responses;
using System.Threading;
using LinqToTwitter;
namespace MssgsDotNetTest
{
    public class Program
    {
        public static AppCredentials creds = new AppCredentials(String.Empty, String.Empty);
        public static List<string> trusted = new List<string>();
        public static Dictionary<string, string> reactions = new Dictionary<string, string>();
        public static string lastMessage = "";
        public static List<string> warned = new List<string>();
        public static List<string> users = new List<string>();
        public static ulong lastId = 0;
        public static int refreshTime = 50;
        public static double lastTime = 0;
        public static string feedkeyword = "#wpsummit";

        public static string GetCommandArgs(string command, string all)
        {
            return all.Replace(command, "").Trim();
        }

        public static void Main(string[] args)
        {
            var client = new MssgsClient();
            client.Open();

            client.Welcomed += (WelcomeMessage welcome) =>
            {
                try
                {
                    client.Authenticate(creds);
                }
                catch (MssgsApiException e)
                {
                    Console.WriteLine(e);
                }
            };

            int dots = 0;
            int show = 0;

            while (!client.IsConnected)
            {
                if (show < 100000000)
                    show++;
                else
                {
                    show = 0;
                    string text = "Waiting for connection";
                    for (int i = 0; i < dots; i++)
                    {
                        text += ".";
                    }
                    Console.WriteLine(text);
                    if (dots > 4) dots = 0;
                    dots++;
                }
            }

            Console.WriteLine("Connected with the Mssgs API on: " + client.Host + ":" + client.Port);
            
            while (!client.IsWelcomed)
            {
                if (show < 100000000)
                    show++;
                else
                {
                    show = 0;
                    string text = "Waiting for welcome";
                    for (int i = 0; i < dots; i++)
                    {
                        text += ".";
                    }
                    Console.WriteLine(text);
                    if (dots > 4) dots = 0;
                    dots++;
                }
            }

            while (!client.IsAuthenticated)
            {
                if (show < 100000000)
                    show++;
                else
                {
                    show = 0;
                    string text = "Waiting for authentication";
                    for (int i = 0; i < dots; i++)
                    {
                        text += ".";
                    }
                    Console.WriteLine(text);
                    if (dots > 4) dots = 0;
                    dots++;
                }
            }
            Console.WriteLine("Authenticated!");
            Console.WriteLine("Please enter a name for the client...");
            string name = Console.ReadLine();

            Console.WriteLine("Ok, " + name + ". Please enter the id of the conversation you wish to join...");
            string conversationId = Console.ReadLine();
            if (conversationId == "debug channel")
                conversationId = "e7f994d5e668b7a9ad7eca7c218c3880";
            var conversation = new MssgsConversation(conversationId);
            Console.WriteLine("Setting up TwitterContext...");
            var ctx = new TwitterContext();
            bool processing = false;
            int max = 20;
            Program.lastTime = GetUnixTime();
            
            client.InternalMessageReceived += (InternalMessage msg) =>
                {
                    Console.WriteLine();
                    Console.WriteLine("Internal Message: " + msg.Type);
                    if(msg.Data != null)
                        foreach (var pair in msg.Data)
                        {
                            Console.WriteLine(pair.Key + ": " + pair.Value);
                        }
                    Console.WriteLine();
                };

            var thread = new Thread(() =>
            {
                while (true)
                {
                    if (GetUnixTime() - Program.lastTime >= Program.refreshTime)
                    {
                        if (processing)
                        {
                            return;
                        }
                        processing = true;
                        List<SearchEntry> results = FetchTweets(ctx, Program.feedkeyword, Program.lastId).Results;
                        Program.lastTime = GetUnixTime();
                        if (results.Count >= 1)
                        {
                            string tweets = "Fetched " + results.Count + " new tweets: \n";
                            Console.WriteLine("Fetched new tweets :)");
                            results.ForEach(tweet =>
                            {
                                if (Program.lastId < tweet.ID)
                                    Program.lastId = tweet.ID;
                                tweets += "\t@" + tweet.FromUser + ": " + tweet.Text + "\n";
                                tweets += tweet.CreatedAt.Day + "/" + tweet.CreatedAt.Month + "/" + tweet.CreatedAt.Year + " ";
                                tweets += tweet.CreatedAt.Hour + ":" + tweet.CreatedAt.Minute + ":" + tweet.CreatedAt.Second;
                                tweets += "\n\n";
                            }
                                );
                            Send(client, tweets);
                        }
                        processing = false;
                    }
                }
            }
           );

            client.MessageReceived += (MssgsMessage msg) =>
                {
                    string data = msg.Op ? "[!" : "[";
                    data += msg.Username + "] " + msg.Message;
                    data += " (" + msg.Date + ")";
                    Console.WriteLine(data);
                    if (!msg.Internal && msg.New && msg.Username != client.Name )
                    {
                        if(msg.Message.StartsWith("--"))
                        {
                            if (!IsTrusted(msg.Username))
                            {
                                
                                //Send(client,msg.Username + " can't perform commands, he isn't trusted :(");
                            }
                            else if(msg.Message.StartsWith("--tweets "))
                            {
                                if (processing)
                                {
                                    Send(client, "Please wait...");
                                    return;
                                }
                                processing = true;
                                string keyword = GetCommandArgs("--tweets", msg.Message);
                                if (keyword.IsFrikkinEmpty())
                                {
                                    if (warned.Contains("kw")) return;
                                    Send(client, "Please provide a keyword!");
                                    warned.Add("kw");
                                    return;
                                }
                                Send(client, "Fetching tweets... Please wait..");
                                var srch = Program.FetchTweets(ctx, keyword);
                                string tweets = "Fetched "+ srch.Results.Count + " tweets: \n";
                                srch.Results.ForEach(tweet =>
                                    {
                                        tweets += "\t@" + tweet.FromUser + ": " + tweet.Text + "\n";
                                        tweets += tweet.CreatedAt.Day + "/" + tweet.CreatedAt.Month + "/" + tweet.CreatedAt.Year + " ";
                                        tweets += tweet.CreatedAt.Hour + ":" + tweet.CreatedAt.Minute + ":" + tweet.CreatedAt.Second;
                                        tweets += "\n\n";
                                    }
                                    );
                                Send(client, tweets);
                                processing = false;
                            }
                            else if (msg.Message.StartsWith("--trending"))
                            {
                                if (processing)
                                {
                                    Send(client, "Please wait...");
                                    return;
                                }
                                processing = true;
                                Send(client, "Fetching trending topics... Please wait..");
                                var trnds = (from trend in ctx.Trends
                                             where trend.Type == TrendType.Daily
                                             select trend).ToList();
                                string tweets = "Fetched "+ max + " of " + trnds.Count + " trending topics: \n";
                                int c = 0;
                                trnds.ForEach(trend =>
                                    {
                                        if (c >= max)
                                            return;
                                        tweets += trend.Name;
                                        tweets += "\n";
                                        c++;
                                    }
                                    );
                                Send(client, tweets);
                                processing = false;
                            }
                            else if (msg.Message.StartsWith("--addtrusted"))
                            {
                                string nm = GetCommandArgs("--addtrusted", msg.Message);
                                if (IsTrusted(nm))
                                    return;
                                else
                                {
                                    AddTrusted(nm);
                                    Send(client, nm + " is now trusted!");
                                }
                            }
                            else if (msg.Message.StartsWith("--rtime "))
                            {
                                int time = Convert.ToInt32(GetCommandArgs("--rtime", msg.Message));
                                Program.refreshTime = time;
                                Send(client, "Refresh time set to " + time);
                            }
                            else if (msg.Message == "--rstop")
                            {
                                if (thread.ThreadState == ThreadState.Stopped)
                                    Send(client, "Refresh thread has been stopped already :')");
                                else
                                {
                                    try
                                    {
                                        thread.Abort();
                                    }
                                    catch { }
                                    Send(client, "Refresh thread stopped.");
                                }
                            }
                            else if (msg.Message == "--rstart")
                            {
                                if (thread.ThreadState == ThreadState.Running)
                                    Send(client, "Refresh thread is already running! :D");
                                else
                                {
                                    try
                                    {
                                        thread.Start();
                                    }
                                    catch { Send(client, "Error occured while starting the refresh thread!"); }
                                    Send(client, "Refresh thread started.");
                                }
                            }
                            else if (msg.Message.StartsWith("--follow "))
                            {
                                string newfollow = GetCommandArgs("--follow", msg.Message);
                                if (newfollow.IsFrikkinEmpty())
                                    return;
                                if (Program.users.Contains(newfollow))
                                    Send(client, "I follow " + newfollow + " already ;-)");
                                else
                                {
                                    Program.users.Add(newfollow);
                                    Send(client, "Following " + newfollow + " now :-D");
                                }
                            }
                            else if (msg.Message.StartsWith("--feed "))
                            {
                                string newfeed = GetCommandArgs("--feed", msg.Message);
                                if (newfeed.IsFrikkinEmpty())
                                    return;
                                if (newfeed != Program.feedkeyword)
                                {
                                    Program.feedkeyword = newfeed;
                                    Send(client, "Updating tweets with keyword: \"" + newfeed + "\" now!:-)");
                                }
                            }
                        }
                    }
                };

            client.AuthUser(name, "http://chinesepaladin.org/images/anigifs/wind1.gif");
            while (!client.IsUserAuthenticated)
            {
                if (show < 100000000)
                    show++;
                else
                {
                    show = 0;
                    string text = "Authing user";
                    for (int i = 0; i < dots; i++)
                    {
                        text += ".";
                    }
                    Console.WriteLine(text);
                    if (dots > 4) dots = 0;
                    dots++;
                }
            }
            Console.WriteLine("Trying to join conversation: " + conversationId);
            client.ConversationJoinFailed += (e) =>
                {
                    Console.WriteLine(e.Message);
                };
            Console.WriteLine("If the chat needs a password please provide it, else hit enter");
            var pw = Console.ReadLine();
            try
            {
                client.Join(conversationId, pw);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            while (!client.InConversation)
            {
                if (show < 100000000)
                    show++;
                else
                {
                    show = 0;
                    string text = "Joining conversation";
                    for (int i = 0; i < dots; i++)
                    {
                        text += ".";
                    }
                    Console.WriteLine(text);
                    if (dots > 4) dots = 0;
                    dots++;
                }
            }
            Console.WriteLine("Joined!");
            Send(client, "Hello!");
            Console.WriteLine("Enter a name to add to the whitelist or skip..");
            string white = Console.ReadLine();
            if (!white.IsFrikkinEmpty())
                AddTrusted(white);
           
            thread.Start();
            while (true)
            {
                var text = Console.ReadLine();
                if (text == "/close")
                    break;
                else if (text.StartsWith("/join "))
                {
                    if (client.InConversation)
                    {
                        Console.WriteLine("already in conversation");
                        return;
                    }
                    string ch = GetCommandArgs("/join", text);
                    if (ch == "old")
                        ch = conversationId;
                    client.Join(ch, "BQAJvj5a4qMSSVj6");
                }
                else if (text.StartsWith("/trusted "))
                {
                    string nm = GetCommandArgs("/trusted", text);
                    if (IsTrusted(nm)) Console.WriteLine(nm + " is trusted already!");
                    else
                    {
                        AddTrusted(nm);
                        Console.WriteLine(nm + " was added to the trusted list :-)");
                    }
                }
                else if (text.StartsWith("/debug"))
                {
                    Console.WriteLine(Program.lastTime);
                    Console.WriteLine(Program.refreshTime);
                    Console.WriteLine(GetUnixTime());
                    Console.WriteLine(GetUnixTime() - Program.lastTime);
                    Console.WriteLine(GetUnixTime() - Program.lastTime >= Program.refreshTime);
                }
                else
                    Send(client, text);
            }
            Send(client, "I'm killing the connection, bai.");
            Console.WriteLine("Closing connection");
            client.Close();
            Console.WriteLine("Connection closed");
            Console.ReadLine();
        }

        public static bool IsTrusted(string name)
        {
            return Program.trusted.Contains(name);
        }

        public static void AddTrusted(string name)
        {
            Program.trusted.Add(name);
        }

        public static bool HasReaction(string keywords)
        {
            foreach (var pair in Program.reactions)
            {
                if (keywords.Contains(pair.Key))
                    return true;
            }
            return false;
        }

        public static string GetReaction(string keywords)
        {
            foreach (var pair in Program.reactions)
            {
                if (keywords.Contains(pair.Key))
                    return pair.Value;
            }
            return "";
        }

        public static void AddReaction(string keywords, string reaction)
        {
            Program.reactions.Add(keywords.Trim(), reaction);
        }

        public static void Send(MssgsClient client, string str)
        {
            if (Program.lastMessage == str)
            {
                Send(client, "Not repeating \"" +str +  "\", oh I just did! -.-");
            }
            client.Send(str);
            Program.lastMessage = str;
        }

        public static Search FetchTweets(TwitterContext ctx, string username, string hashtag, ulong since)
        {
            return (from search in ctx.Search
                        where search.Type == SearchType.Search &&
                        search.PersonFrom == username &&
                        search.Query == hashtag &&
                        search.SinceID == since
                        select search).Single();
        }

        public static Search FetchTweets(TwitterContext ctx,string hashtag, ulong since)
        {
            return (from search in ctx.Search
                    where search.Type == SearchType.Search &&
                    search.Query == hashtag &&
                    search.SinceID == since
                    select search).Single();
        }

        public static Search FetchTweets(TwitterContext ctx, string hashtag)
        {
            return (from search in ctx.Search
                    where search.Type == SearchType.Search &&
                    search.Query == hashtag 
                    select search).Single();
        }

        public static double GetUnixTime()
        {
            return (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        }
    }
}
