// Produces an integer that represents the resultant clues from a guess
// treating the clues as a base-3 number, where ⬛ is a 0, 🟨🟦 is a 1,
// and 🟩🟧 is a 2
//
// -1 is an invalid guess
int CheckGuess(string target, string guess)
{
  if (target.Length != guess.Length) return -1;

  var gLets = guess.ToList();
  var tLets = target.ToList();
  var res = Enumerable.Repeat(0, target.Length).ToList();

  for (int i = 0; i < gLets.Count; i++)
  {

  }
}