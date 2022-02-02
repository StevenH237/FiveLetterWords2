var words = File.ReadLines("data/words_filter.txt").ToList();

var wrong = '.';
var misplaced = '-';
var right = '+';

// Produces an integer that represents the resultant clues from a guess
// treating the clues as a base-3 number, where ⬛ is a 0, 🟨🟦 is a 1,
// and 🟩🟧 is a 2
//
// null is an invalid guess (size mismatch)
string? CheckGuess(string guess, string target)
{
  if (target.Length != guess.Length) return null;

  var gLets = guess.ToList();
  var tLets = target.ToList();
  var res = Enumerable.Repeat(wrong, target.Length).ToArray();
  var offsets = Enumerable.Range(0, gLets.Count).ToList();

  for (int i = 0; i < gLets.Count; i++)
  {
    if (gLets[i] == tLets[i])
    {
      res[offsets[i]] = right;
      offsets.RemoveAt(i);
      gLets.RemoveAt(i);
      tLets.RemoveAt(i);
      i--;
    }
  }

  for (int i = 0; i < gLets.Count; i++)
  {
    int loc = tLets.IndexOf(gLets[i]);

    if (loc != -1)
    {
      res[offsets[i]] = misplaced;
      offsets.RemoveAt(i);
      gLets.RemoveAt(i);
      tLets.RemoveAt(loc);
      i--;
    }
  }

  return new string(res);
}

// Splits a list of words into buckets for a single target word. Buckets
// are grouped by the result.
Dictionary<string, List<string>> SplitBuckets(IEnumerable<string> guesses, string target)
{
  return guesses
    .Aggregate(new Dictionary<string, List<string>>(), (dict, itm) =>
    {
      if (itm == target) return dict;
      var score = CheckGuess(itm, target);
      if (score == null) return dict;
      if (!dict.ContainsKey(score))
        dict[score] = new List<string>();
      dict[score].Add(itm);
      return dict;
    });
}

// Console.WriteLine(CheckGuess("owoli", "uwulo")); // expected value is 12020_3 or 141

// // guess we need this too
// T MedianBy<T>(IEnumerable<T> inputs, Func<T, int> function)
// {
//   var orderedInputs = inputs
//     .Select(x => (Value: function(x), Item: x))
//     .OrderBy(x => x.Value)
//     .ToList();
// 
//   var count = orderedInputs.Count;
//   return orderedInputs[count / 2].Item;
// }
// 
// var buckets = SplitBuckets(words, "aeons");
// foreach (var bucket in buckets)
// {
//   Console.WriteLine($"{bucket.Key}: {bucket.Value.Count} (like {bucket.Value[0]})");
// }
// 
// var highest = buckets.MaxBy(x => x.Value.Count);
// Console.WriteLine($"Highest: {highest.Key} ({highest.Value.Count})");
// var median = MedianBy(buckets, x => x.Value.Count);
// Console.WriteLine($"Median: {median.Key} ({median.Value.Count})");

(string Word, Dictionary<string, List<string>> Buckets) FindOptimalWord(IList<string> words)
{
  var buckets = words.Select(x => (Word: x, Buckets: SplitBuckets(words, x)));
  return buckets.MinBy(x => x.Buckets.Max(y => y.Value.Count));
}

// Now let's sort the whole list by optimized play recursively:
List<string> OptimizeBucket(List<string> words)
{
  List<string> ret = new();

  var optimal = FindOptimalWord(words);
  ret.Add(optimal.Word);
  foreach (var bucket in optimal.Buckets.Select(x => x.Value).OrderByDescending(x => x.Count))
  {
    if (bucket.Count < 3) ret.AddRange(bucket);
    else ret.AddRange(OptimizeBucket(bucket));
  }

  return ret;
}

File.WriteAllLines("data/words_filter_optimal.txt", OptimizeBucket(words));