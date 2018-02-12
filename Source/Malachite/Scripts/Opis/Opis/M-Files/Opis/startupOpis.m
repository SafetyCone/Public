%% Project path definition.

% Absolute path to this project's root directory.
projectDirectoryPath = cd;


%% Add library paths.

libraryPaths = ...
    { ...
    getPublicCommonLibraryPath();
    };

addLibraryPaths(libraryPaths);

clear libraryPaths;


%% Other directory paths.