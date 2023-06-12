using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geradorDeSenha
{
    public class PasswordGenerator
    {
        private const string LowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
        private const string UppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Digits = "0123456789";
        private const string SpecialCharacters = "!@#$%^&*()";

        public static string GeneratePassword(int length, bool includeLowercase, bool includeUppercase, bool includeDigits, bool includeSpecialCharacters)
        {
            StringBuilder characterSet = new StringBuilder();

            // Constrói o conjunto de caracteres com base nas opções selecionadas
            if (includeLowercase)
                characterSet.Append(LowercaseLetters);

            if (includeUppercase)
                characterSet.Append(UppercaseLetters);

            if (includeDigits)
                characterSet.Append(Digits);

            if (includeSpecialCharacters)
                characterSet.Append(SpecialCharacters);

            if (characterSet.Length == 0)
            {
                throw new ArgumentException("Pelo menos um conjunto de caracteres deve ser selecionado.");
            }

            using (RNGCryptoServiceProvider cryptoProvider = new RNGCryptoServiceProvider())
            {
                byte[] randomNumber = new byte[length];
                cryptoProvider.GetBytes(randomNumber);

                char[] password = new char[length];
                int maxRandomNumber = characterSet.Length;

                // Gera cada caractere da senha aleatoriamente
                for (int i = 0; i < length; i++)
                {
                    int randomIndex = randomNumber[i] % maxRandomNumber;
                    password[i] = characterSet[randomIndex];
                }

                return new string(password);
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Gerador de Senhas");
            Console.WriteLine("------------------");

            int length = GetPasswordLength();
            bool includeLowercase = GetYesNoAnswer("Incluir letras minúsculas? (S/N): ");
            bool includeUppercase = GetYesNoAnswer("Incluir letras maiúsculas? (S/N): ");
            bool includeDigits = GetYesNoAnswer("Incluir números? (S/N): ");
            bool includeSpecialCharacters = GetYesNoAnswer("Incluir caracteres especiais? (S/N): ");

            try
            {
                // Gera a senha com base nas opções selecionadas
                string password = PasswordGenerator.GeneratePassword(length, includeLowercase, includeUppercase, includeDigits, includeSpecialCharacters);
                Console.WriteLine("\nSenha gerada: " + password); Console.ReadLine();
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Erro: " + ex.Message); Console.ReadLine();
            }

            Console.WriteLine("\nPressione qualquer tecla para sair..."); Console.ReadLine();
            Console.ReadKey();
        }

        private static int GetPasswordLength()
        {
            Console.Write("Informe o comprimento desejado da senha: ");
            string input = Console.ReadLine();
            int length;

            // Obtém o comprimento da senha digitado pelo usuário
            while (!int.TryParse(input, out length) || length <= 0)
            {
                Console.Write("Comprimento inválido. Por favor, informe um valor inteiro positivo: ");
                input = Console.ReadLine();
            }

            return length;
        }

        private static bool GetYesNoAnswer(string message)
        {
            Console.Write(message);
            string input = Console.ReadLine();

            // Obtém uma resposta "S" ou "N" do usuário
            while (!input.Equals("S", StringComparison.OrdinalIgnoreCase) && !input.Equals("N", StringComparison.OrdinalIgnoreCase))
            {
                Console.Write("Resposta inválida. Por favor, digite S para Sim ou N para Não: ");
                input = Console.ReadLine();
            }

            return input.Equals("S", StringComparison.OrdinalIgnoreCase);
        }
    }

}


