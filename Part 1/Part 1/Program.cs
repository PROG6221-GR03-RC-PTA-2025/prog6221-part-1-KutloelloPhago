using System;
using System.Speech.Synthesis;
using System.Threading;
using System.Collections.Generic;

namespace CyberSecurityBot
{
    
        internal class Program
        {
            private static string currentTopic = "";
            private static int followUpCount = 0;
            private static List<string> recentTopics = new List<string>();
            private static Random random = new Random();
            private static string userName = "";
            private static List<string> userInterests = new List<string>();
            private static Dictionary<string, string> userDetails = new Dictionary<string, string>();

            static void Main()
            {
                // Start a new thread to display ASCII art while speaking
                Thread displayAsciiThread = new Thread(DisplayCyberDroidAscii);
                displayAsciiThread.Start();

                // Initialize the SpeechSynthesizer for high-quality audio output
                SpeechSynthesizer synth = new SpeechSynthesizer
                {
                    Volume = 100, // Setting volume to maximum (0-100)
                    Rate = 0 // Setting speech rate (0 is neutral)
                };

                // Greeting message with audio
                synth.Speak("Hello User! I am Cyber Droid, your friendly Cyber Security Awareness Bot, and I am here to help you stay safe online!");

                // Ask the user for their name
                do
                {
                    Console.Write("Please Enter your name to start: ");
                    userName = Console.ReadLine()?.Trim();

                    if (string.IsNullOrEmpty(userName))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Please enter your name to continue.");
                        Console.ResetColor();
                    }
                } while (string.IsNullOrEmpty(userName));
                userDetails["name"] = userName; // Store the name

                // Welcome message with personalized greeting
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"\nHello, {userName}! That's a cool name! Welcome to the cybersecurity awareness bot.");
                Console.ResetColor();

                // Ask the user to ask questions
                Console.WriteLine("Feel free to ask me any cybersecurity questions! To exit, type 'exit' at any time.");

                // Main conversation loop
                while (true)
                {
                    Console.Write("You: ");
                    string userInput = Console.ReadLine()?.Trim();

                    if (string.IsNullOrEmpty(userInput))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        if (!string.IsNullOrEmpty(currentTopic))
                        {
                            Console.WriteLine($"Cyber Droid: I'm still discussing {currentTopic}. Did you want to continue or switch topics?");
                        }
                        else
                        {
                            Console.WriteLine("Cyber Droid: I didn't hear anything. You can ask about cybersecurity topics or type 'exit' to leave.");
                        }
                        Console.ResetColor();
                        continue;
                    }

                    if (userInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("Goodbye! Stay safe online.");
                        Console.ResetColor();
                        break;
                    }

                    // Get and display response
                    Console.Write("Cyber Droid: ");
                    string response = GetBasicResponse(userInput);
                    SimulateTypingEffect(response);
                    Console.WriteLine("\n");
                }

                // Wait for the ASCII art thread to complete
                displayAsciiThread.Join();

                // Farewell message
                string goodbye = "Goodbye! Stay safe online.";
                Console.WriteLine(goodbye);
                synth.Speak(goodbye);
            }

            // Simulate typing effect to make the bot's responses feel more dynamic
            static void SimulateTypingEffect(string message)
            {
                foreach (char c in message)
                {
                    Console.Write(c);
                    Thread.Sleep(50); // Typing delay for effect
                }
            }

            // Get the appropriate response based on user input
            static string GetBasicResponse(string input)
            {
                // Normalize input to lowercase and trimmed once
                string normalizedInput = input?.Trim().ToLower() ?? "";

                // First check if user wants to transition topics
                if (normalizedInput.Contains("switch") || normalizedInput.Contains("change") || normalizedInput.Contains("new topic"))
                {
                    string oldTopic = currentTopic;
                    currentTopic = "";
                    followUpCount = 0;
                    SetTextColor(ConsoleColor.DarkCyan);
                    return $"Okay, let's switch from {oldTopic}. What would you like to discuss instead?";
                }

                if ((normalizedInput.Contains("go back") || normalizedInput.Contains("previous")) && recentTopics.Count > 1)
                {
                    currentTopic = recentTopics[recentTopics.Count - 2];
                    recentTopics.RemoveAt(recentTopics.Count - 1);
                    SetTextColor(ConsoleColor.DarkCyan);
                    return $"Let's go back to {currentTopic}. What would you like to know?";
                }

                // Handle follow-ups if we're in a topic
                if (!string.IsNullOrEmpty(currentTopic))
                {
                    followUpCount++;

                    if (normalizedInput.Contains("more") || normalizedInput.Contains("yes") ||
                        normalizedInput.Contains("explain") || normalizedInput.Contains("detail"))
                    {
                        return GetFollowUpResponse(currentTopic, followUpCount);
                    }

                    if (normalizedInput.Contains("example") || normalizedInput.Contains("show me"))
                    {
                        SetTextColor(ConsoleColor.DarkYellow);
                        return GetExampleForTopic(currentTopic);
                    }

                    if (normalizedInput.Contains("no") || normalizedInput.Contains("enough"))
                    {
                        string oldTopic = currentTopic;
                        currentTopic = "";
                        SetTextColor(ConsoleColor.DarkCyan);
                        return $"Okay, we can stop discussing {oldTopic}. What else would you like to know about?";
                    }
                }

                // Check for more specific phrases first
                if (normalizedInput.Contains("password safety"))
                {
                    RememberUserInterest("password safety");
                    currentTopic = "password safety";
                    recentTopics.Add(currentTopic);
                    followUpCount = 0;
                    SetTextColor(ConsoleColor.Red);
                    return $"{GetPersonalizedResponse("password safety")}Always use strong, unique passwords for each account. Consider using a password manager to securely store your passwords.";
                }

                if (normalizedInput.Contains("two-step authentication") || normalizedInput.Contains("2fa"))
                {
                    RememberUserInterest("2fa");
                    currentTopic = "2fa";
                    recentTopics.Add(currentTopic);
                    followUpCount = 0;
                    SetTextColor(ConsoleColor.Yellow);
                    return $"{GetPersonalizedResponse("2fa")}Two-Step Authentication (2FA) adds an extra layer of security by requiring a second form of verification, like a code sent to your phone.";
                }

                if (normalizedInput.Contains("social engineering"))
                {
                    RememberUserInterest("social engineering");
                    currentTopic = "social engineering";
                    recentTopics.Add(currentTopic);
                    followUpCount = 0;
                    SetTextColor(ConsoleColor.Magenta);
                    return $"{GetPersonalizedResponse("social engineering")}Social engineering attacks trick users into revealing confidential information. Always be cautious with unsolicited requests and verify identities.";
                }

                if (normalizedInput.Contains("data breach"))
                {
                    RememberUserInterest("data breach");
                    currentTopic = "data breach";
                    recentTopics.Add(currentTopic);
                    followUpCount = 0;
                    SetTextColor(ConsoleColor.Red);
                    return $"{GetPersonalizedResponse("data breach")}A data breach can expose sensitive information. Use strong passwords and monitor your accounts regularly for suspicious activity.";
                }

                if (normalizedInput.Contains("phishing"))
                {
                    RememberUserInterest("phishing");
                    currentTopic = "phishing";
                    recentTopics.Add(currentTopic);
                    followUpCount = 0;
                    SetTextColor(ConsoleColor.Magenta);
                    return $"{GetPersonalizedResponse("phishing")}Phishing is a type of scam where attackers try to trick you into revealing sensitive information, like passwords or credit card numbers. Always verify the sender's identity before clicking on links.";
                }

                if (normalizedInput.Contains("malware"))
                {
                    RememberUserInterest("malware");
                    currentTopic = "malware";
                    recentTopics.Add(currentTopic);
                    followUpCount = 0;
                    SetTextColor(ConsoleColor.Red);
                    return $"{GetPersonalizedResponse("malware")}Malware refers to malicious software like viruses or spyware. Protect yourself by keeping your software updated and using reliable antivirus software.";
                }

                if (normalizedInput.Contains("encryption"))
                {
                    RememberUserInterest("encryption");
                    currentTopic = "encryption";
                    recentTopics.Add(currentTopic);
                    followUpCount = 0;
                    SetTextColor(ConsoleColor.Cyan);
                    return $"{GetPersonalizedResponse("encryption")}Encryption protects data by encoding it so only authorized parties can read it. Always use encrypted connections and storage where possible.";
                }

                if (normalizedInput.Contains("vpn"))
                {
                    RememberUserInterest("vpn");
                    currentTopic = "vpn";
                    recentTopics.Add(currentTopic);
                    followUpCount = 0;
                    SetTextColor(ConsoleColor.Green);
                    return $"{GetPersonalizedResponse("vpn")}A VPN (Virtual Private Network) helps secure your internet connection by encrypting your data and masking your IP address.";
                }

                if (normalizedInput.Contains("firewall"))
                {
                    RememberUserInterest("firewall");
                    currentTopic = "firewall";
                    recentTopics.Add(currentTopic);
                    followUpCount = 0;
                    SetTextColor(ConsoleColor.Yellow);
                    return $"{GetPersonalizedResponse("firewall")}Firewalls monitor and control incoming and outgoing network traffic to protect your devices from unauthorized access.";
                }

                if (normalizedInput.Contains("backup") || normalizedInput.Contains("data backup"))
                {
                    RememberUserInterest("backup");
                    currentTopic = "backup";
                    recentTopics.Add(currentTopic);
                    followUpCount = 0;
                    SetTextColor(ConsoleColor.Green);
                    return $"{GetPersonalizedResponse("backup")}Regular backups are essential to recover your data in case of hardware failure, cyberattacks, or accidental deletion.";
                }

                if (normalizedInput.Contains("scam"))
                {
                    RememberUserInterest("scam");
                    currentTopic = "scam";
                    recentTopics.Add(currentTopic);
                    followUpCount = 0;
                    SetTextColor(ConsoleColor.Magenta);
                    return $"{GetPersonalizedResponse("scam")}Be cautious of scams, especially phishing attempts. Always verify the sender's identity before clicking on links or providing personal information.";
                }

                if (normalizedInput.Contains("privacy"))
                {
                    RememberUserInterest("privacy");
                    currentTopic = "privacy";
                    recentTopics.Add(currentTopic);
                    followUpCount = 0;
                    SetTextColor(ConsoleColor.Cyan);
                    return $"{GetPersonalizedResponse("privacy")}To protect your privacy online, be mindful of the information you share on social media and use privacy settings to control who can see your data.";
                }

                // Handle common myths
                if (normalizedInput.Contains("common myth") || normalizedInput.Contains("cybersecurity myth"))
                {
                    currentTopic = "myths";
                    recentTopics.Add(currentTopic);
                    followUpCount = 0;
                    SetTextColor(ConsoleColor.Yellow);
                    List<string[]> mythsAndFacts = new List<string[]>
                {
                    new string[] { "Myth: Using a simple password is enough.", "Fact: Strong, complex passwords are essential for security." },
                    new string[] { "Myth: Antivirus software is all you need.", "Fact: Regular updates and safe browsing habits are also crucial." },
                    new string[] { "Myth: Public Wi-Fi is safe to use without precautions.", "Fact: Public Wi-Fi can expose you to security risks; use a VPN." },
                    new string[] { "Myth: Cybersecurity is only a concern for large companies.", "Fact: Individuals are often targeted by cybercriminals as well." },
                    new string[] { "Myth: You can always trust emails from known contacts.", "Fact: Email accounts can be compromised; verify unexpected requests." }
                };
                    int index = random.Next(mythsAndFacts.Count);
                    return $"{GetPersonalizedResponse("myths")}{mythsAndFacts[index][0]}\n{mythsAndFacts[index][1]}";
                }

                // Handle memory recall
                if (normalizedInput.Contains("what do you know about me") || normalizedInput.Contains("remember about me"))
                {
                    return GetRememberedInfo();
                }

                // Basic responses to common cybersecurity-related questions
                if (normalizedInput.Contains("how are you"))
                {
                    SetTextColor(ConsoleColor.Cyan);
                    return $"{userName}, I don't have emotions, but I'm always here to assist you in staying safe online!";
                }

                if (normalizedInput.Contains("what's your purpose") || normalizedInput.Contains("what is your purpose"))
                {
                    SetTextColor(ConsoleColor.Green);
                    return $"{userName}, my purpose is to provide you with valuable cybersecurity tips and help you stay protected while browsing the internet.";
                }

                if (normalizedInput.Contains("what can i ask") || normalizedInput.Contains("what can you do"))
                {
                    SetTextColor(ConsoleColor.Yellow);
                    return $"{userName}, you can ask about password safety, phishing attacks, malware, encryption, VPNs, and much more!";
                }

                // Default response based on context
                if (!string.IsNullOrEmpty(currentTopic))
                {
                    SetTextColor(ConsoleColor.Gray);
                    return $"{userName}, I was discussing {currentTopic}. Did you want more information about that, or should we switch topics?";
                }

                // Default fallback if no specific topic matches
                SetTextColor(ConsoleColor.Gray);
                return $"{userName}, I didn't quite understand that. Could you ask about another topic, like phishing or password safety?";
            }

            static string GetFollowUpResponse(string topic, int depth)
            {
                switch (topic.ToLower())
                {
                    case "phishing":
                        if (depth == 1)
                            return $"{userName}, common signs of phishing include:\n- Urgent or threatening language\n- Misspelled URLs or email addresses\n- Requests for sensitive information\n- Generic greetings like 'Dear Customer'\nWould you like to see an example?";
                        else if (depth == 2)
                            return $"{userName}, example phishing email:\n'Urgent! Your account will be suspended. Click here to verify your details.'\n\nAlways hover over links to check the real URL before clicking.";
                        else if (depth == 3)
                            return $"{userName}, you can report phishing attempts to:\n1. Your email provider\n2. The Anti-Phishing Working Group (reportphishing@apwg.org)\n3. The FTC at reportfraud.ftc.gov\nShould we discuss how to protect yourself from phishing?";
                        break;

                    case "password safety":
                    case "passwords":
                        if (depth == 1)
                            return $"{userName}, a strong password should:\n- Be at least 12 characters\n- Include uppercase, lowercase, numbers, and symbols\n- Avoid personal information\n- Be unique for each account\nWant to know about password managers?";
                        else if (depth == 2)
                            return $"{userName}, password managers can:\n- Generate strong passwords\n- Store them securely\n- Auto-fill them for you\n\nPopular options: LastPass, 1Password, Bitwarden\nWould you like installation tips?";
                        break;

                    case "malware":
                        if (depth == 1)
                            return $"{userName}, common malware types:\n- Viruses (spread by infecting files)\n- Ransomware (locks your files)\n- Spyware (steals information)\n- Trojans (disguised as legitimate software)\nNeed prevention tips?";
                        break;
                }

                currentTopic = "";
                followUpCount = 0;
                return $"{userName}, is there another cybersecurity topic you'd like to discuss?";
            }

            static string GetExampleForTopic(string topic)
            {
                switch (topic.ToLower())
                {
                    case "password safety":
                        return $"{userName}, WEAK PASSWORD EXAMPLES:\n- password123\n- 123456\n- qwerty\n- yourname1980\n\nSTRONG EXAMPLES:\n- B1ue\\$ky!Runn3r\n- 7Dolphins@Moon\n- C@tL0v3r42!";

                    case "2fa":
                    case "two-factor authentication":
                        return $"{userName}, EXAMPLE 2FA SCENARIO:\n1. You enter your password\n2. A code is sent to your phone\n3. You enter the code\n4. Only then are you logged in\n\nEven if someone steals your password, they can't login without the code!";

                    default:
                        return $"{userName}, here's an example related to {topic}... (I'll need more specific examples added for this topic)";
                }
            }

            // Helper function to set the console text color
            static void SetTextColor(ConsoleColor color)
            {
                Console.ForegroundColor = color;
            }

            // Memory-related helper methods
            static void RememberUserInterest(string topic)
            {
                if (!userInterests.Contains(topic))
                {
                    userInterests.Add(topic);
                    SetTextColor(ConsoleColor.Cyan);
                    Console.Write("Cyber Droid: ");
                    SimulateTypingEffect($"I'll remember you're interested in {topic}. This will help me give you better advice!");
                    Console.WriteLine("\n");
                    Console.ResetColor();
                }
            }

            static string GetPersonalizedResponse(string topic)
            {
                if (userInterests.Contains(topic))
                {
                    return $"{userName}, since you're interested in {topic}, here's something you might find useful: ";
                }
                return $"{userName}, ";
            }

            static string GetRememberedInfo()
            {
                if (userDetails.Count == 0 && userInterests.Count == 0)
                {
                    return $"{userName}, I don't remember any specific details about you yet. Tell me something about yourself!";
                }

                string response = $"{userName}, ";
                if (userDetails.ContainsKey("name"))
                {
                    response += $"I know your name is {userDetails["name"]}. ";
                }
                if (userInterests.Count > 0)
                {
                    response += $"You're interested in {string.Join(", ", userInterests)}. ";
                }
                return response + "Is there anything else you'd like me to remember?";
            }

            // Display colorful ASCII art to enhance the user experience
            static void DisplayCyberDroidAscii()
            {
                string cyberDroidAscii = @"
 CCCC   Y   Y  BBBBB   EEEEE  RRRR       DDDD   RRRR    OOO   III   DDDD  
C       Y Y   B    B  E       R   R      D   D  R   R  O   O   I    D   D 
C        Y    BBBBB   EEEE    RRRR       D   D  RRRR   O   O   I    D   D 
C        Y    B    B  E       R  R       D   D  R  R   O   O   I    D   D 
 CCCC    Y    BBBBB   EEEEE   R   R      DDDD   R   R   OOO   III   DDDD  
";

                // Define colors for the rainbow effect
                ConsoleColor[] rainbowColors = {
                ConsoleColor.Red,
                ConsoleColor.Yellow,
                ConsoleColor.Green,
                ConsoleColor.Cyan,
                ConsoleColor.Blue,
                ConsoleColor.Magenta,
                ConsoleColor.White
            };

                int colorIndex = 0;
                // Print the ASCII art with a rainbow effect
                foreach (char c in cyberDroidAscii)
                {
                    if (c == '\n')
                    {
                        Console.WriteLine(); // Move to next line without changing color
                    }
                    else
                    {
                        Console.ForegroundColor = rainbowColors[colorIndex % rainbowColors.Length];
                        Console.Write(c);
                        colorIndex++;
                    }
                }
                Console.ResetColor(); // Reset color after displaying ASCII art
            }
        }
    }