using System;
using System.Speech.Synthesis;
using System.Threading;
using System.Collections.Generic;

namespace CyberSecurityBot
{
    internal class Program
    {
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
            string userName;
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

            // Welcome message with personalized greeting
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"\nHello, {userName}! That's a cool name! Welcome to the cybersecurity awareness bot.");
            Console.ResetColor();

            // Ask the user to ask questions
            Console.WriteLine("Feel free to ask me any cybersecurity questions! To exit, type 'exit' at any time.");

            // Start main conversation loop
            while (true)
            {
                Console.Write("You: ");
                string userInput = Console.ReadLine()?.Trim().ToLower();

                if (string.IsNullOrEmpty(userInput))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("You: I didn’t quite understand that. Could you please rephrase your question?");
                    Console.ResetColor();
                    continue;
                }

                // Exit condition
                if (userInput == "exit")
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Goodbye! Stay safe online.");
                    Console.ResetColor();
                    break;
                }

                // Simulate typing effect for the response
                Console.Write("Cyber Droid: ");
                SimulateTypingEffect(GetBasicResponse(userInput));
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
            // Basic responses to common cybersecurity-related questions
            if (input.Contains("how are you"))
            {
                SetTextColor(ConsoleColor.Cyan);
                return "I don’t have emotions, but I’m always here to assist you in staying safe online!";
            }

            if (input.Contains("what's your purpose") || input.Contains("what is your purpose"))
            {
                SetTextColor(ConsoleColor.Green);
                return "My purpose is to provide you with valuable cybersecurity tips and help you stay protected while browsing the internet.";
            }

            if (input.Contains("what can i ask"))
            {
                SetTextColor(ConsoleColor.Yellow);
                return "You can ask about password safety, phishing attacks, malware, encryption, VPNs, and much more!";
            }

            // Handle specific topics like password safety, phishing, etc.
            if (input.Contains("password safety"))
            {
                SetTextColor(ConsoleColor.Red);
                return "Always use strong, unique passwords for each account. Consider using a password manager to securely store your passwords.";
            }

            if (input.Contains("phishing"))
            {
                SetTextColor(ConsoleColor.Magenta);
                return "Phishing is a type of scam where attackers try to trick you into revealing sensitive information, like passwords or credit card numbers. Always verify the sender's identity before clicking on links.";
            }

            if (input.Contains("malware"))
            {
                SetTextColor(ConsoleColor.Red);
                return "Malware refers to malicious software like viruses or spyware. Protect yourself by keeping your software updated and using reliable antivirus software.";
            }

            if (input.Contains("two-step authentication") || input.Contains("2fa"))
            {
                SetTextColor(ConsoleColor.Yellow);
                return "Two-Step Authentication (2FA) adds an extra layer of security by requiring a second form of verification, like a code sent to your phone.";
            }

            // Default fallback if no specific topic matches
            SetTextColor(ConsoleColor.Gray);
            return "I didn't quite understand that. Could you ask about another topic, like phishing or password safety?";
        }

        // Display password safety precautions for storing passwords on your phone
        static void DisplayPasswordSafetyPrecautions()
        {
            string[] precautions = {
                "1. Use a Password Manager to store passwords securely.",
                "2. Enable Device Encryption to protect your data.",
                "3. Use a strong PIN, password, or biometric authentication for your phone.",
                "4. Enable Two-Factor Authentication (2FA) on accounts that support it.",
                "5. Be cautious when downloading apps and check their permissions.",
                "6. Keep your phone's operating system and apps updated."
            };

            SetTextColor(ConsoleColor.Yellow);
            Console.WriteLine("\nHere are some precautions for storing passwords safely on your phone:");
            foreach (var precaution in precautions)
            {
                Console.WriteLine(precaution);
            }
            Console.ResetColor();
        }

        // Helper function to set the console text color
        static void SetTextColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
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
