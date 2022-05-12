mergeInto(LibraryManager.library, {
  FBGameStart: function () {
    analytics.logEvent("Game Start", {});
  },
  FBWrongTime: function (report) {
    analytics.logEvent("Incorrect Time Made", {
      report: Pointer_stringify(report),
    });
  },
  FBScoreTime: function (report) {
    analytics.logEvent("Player Scored", {
      report: Pointer_stringify(report),
    });
  },
  FBFinalScore: function (report) {
    analytics.logEvent("Final Score", {
      report: Pointer_stringify(report),
    });
  },
});
