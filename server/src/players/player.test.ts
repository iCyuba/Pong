import Player from "@/players/player";

/** Don't ask. Just use them */
export const usernames = [
  "test",
  "what",
  "i dont even know",
  "16 characters ^^",
  "a",
  "why did i",
  "even make this",
  "stupid list",
  "of names?",
  "---",
  "oh, special",
  "characters are",
  "supported as",
  "well.",
  "~±<+-;œ∑´®†¥¨ˆø",
  "i value my time",
  "as you can see",
  "from this list",
  "of creative",
  "usernames <3",
];

describe("Offline tests", () => {
  describe("Player.validateName and Player.safeValidateName", () => {
    // Try validating all sorts of garbage. What is the point of this??
    test.each([undefined, 123, /a/, null, false, NaN, Infinity, () => {}])(
      "Non-string: %s",
      (value: unknown) => {
        // Calling Player.validateName should throw.
        expect(() => Player.validateName(value as string)).toThrowError(
          `A name must be a string (got ${typeof value})`
        );

        // Calling Player.safeValidateName should return false
        expect(Player.safeValidateName(value as string)).toBe(false);
      }
    );

    // ""....
    // I'm not gonna comment this. Read the code (if u want, idc)
    test("0 length string", () => {
      expect(() => Player.validateName("")).toThrowError("A name must not be empty");

      expect(Player.safeValidateName("")).toBe(false);
    });

    // AAAAAAAAAAAAAAAAAAAA (for example)
    test("Too long", () => {
      const name = "AAAAAAAAAAAAAAAAAAAA";

      expect(() => Player.validateName(name)).toThrowError(
        "A name must not be longer than 16 characters"
      );

      expect(Player.safeValidateName(name)).toBe(false);
    });

    // Very great (valid) names
    test.each(usernames)("Valid: %s", name => {
      expect(() => Player.validateName(name)).not.toThrow();

      expect(Player.safeValidateName(name)).toBe(true);
    });
  });
});
