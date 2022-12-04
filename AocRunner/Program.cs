var day = new Day3();
#if (DEBUG)
Solve1(day, test:true);
Solve2(day, test:true);

#elif (RELEASE)
Solve1(day, askForSubmit:true);
Solve2(day, askForSubmit:true);
#endif
