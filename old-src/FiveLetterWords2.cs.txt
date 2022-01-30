// See https://aka.ms/new-console-template for more information
List<string> words = File.ReadLines("data/words_alpha.txt").Where(x => x.Length == 5).ToList();

Dictionary<char, int> frequency = new Dictionary<char, int>() {
  {'s', 0}, {'p', 0}, {'h', 0}, {'i', 0}, {'n', 0}, {'x', 0},
  {'o', 0}, {'f', 0},
  {'b', 0}, {'l', 0}, {'a', 0}, {'c', 0}, {'k', 0},
  {'q', 0}, {'u', 0}, /*  a  */ {'r', 0}, {'t', 0}, {'z', 0}, //,
  {'j', 0}, /*  u  */ {'d', 0}, {'g', 0}, {'e', 0},
  {'m', 0}, {'y', 0},
  {'v', 0}, /*  o  */ {'w', 0} //!
};

// Get the frequencies first
foreach (string word in words)
{
  foreach (char chr in word)
  {
    frequency[chr] += 1;
  }
}

/*
// Having nothing else to do so far, let's print the frequencies from largest to smallest
foreach (var e in frequency.OrderByDescending(x => x.Value))
{
  Console.WriteLine($"{e.Key}: {e.Value} times");
}
*/

words.Sort((left, right) =>
{
  // First, how many unique letters do these words have?
  int leftLetters = left.Distinct().Count();
  int rightLetters = right.Distinct().Count();
  // This is reversed for a descending sort.
  if (leftLetters != rightLetters) return rightLetters.CompareTo(leftLetters);

  // What's the total score of each word?
  // That's the sum of the frequencies of its letters.
  int leftScore = left.Aggregate(0, (Σ, c) => Σ + frequency[c]);
  int rightScore = right.Aggregate(0, (Σ, c) => Σ + frequency[c]);
  // Also reversed for a descending sort.
  if (leftScore != rightScore) return rightScore.CompareTo(leftScore);

  // Otherwise just compare with letters sorted by frequency
  for (int i = 0; i < 5; i++)
  {
    int lCharScore = frequency[left[i]];
    int rCharScore = frequency[right[i]];

    if (lCharScore != rCharScore) return lCharScore.CompareTo(rCharScore);
  }

  return 0;
});

File.WriteAllLines("data/words_sorts.txt", words);
