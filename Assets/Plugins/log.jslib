mergeInto(LibraryManager.library, {

    FBWrongTime:function(time) {
        analytics.logEvent("Incorrect time made", { 
            time: Pointer_stringify(time),
         });
    },
});