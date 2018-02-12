function filePaths = filePathsByExtensions(directoryPath, fileExtensions)
% filePathsByExtension - Gets all file paths in a directory which have the
% specified file extensions.


%%

% Build the regex pattern.
nImageFileExtensions = numel(fileExtensions);
if(0 == nImageFileExtensions)
   error('No image file extensions specified.'); 
end
extensionsPattern = fileExtensions{1};
for iFileExtension = 2:nImageFileExtensions
    fileExtension = fileExtensions{iFileExtension};
    extensionsPattern = strcat(extensionsPattern, '|', fileExtension);
end


% Determine which directory items are of interest.
listing = dir(directoryPath);
nDirectoryItems = numel(listing);
filesOfInterestLogicals = false(nDirectoryItems, 1);
for iDirectoryItem = 1:nDirectoryItems
    directoryItem = listing(iDirectoryItem);
    if not(directoryItem.isdir) % No need to look at directories.
        if not(filesOfInterestLogicals(iDirectoryItem)) % If we have not already established this item as being of interest.
            fileNameLowered = lower(directoryItem.name);
            matchIndices = regexp(fileNameLowered, extensionsPattern, 'once');
            if(not(isempty(matchIndices)))
                filesOfInterestLogicals(iDirectoryItem) = true;
            end
        end
    end
end


% Get the file paths of interest.
nFilesOfInterest = sum(filesOfInterestLogicals);
filePaths = cell(nFilesOfInterest, 1);
iFilePathCounter = 1;
for iDirectoryItem = 1:nDirectoryItems
    fileIsOfInterest = filesOfInterestLogicals(iDirectoryItem);
    if(fileIsOfInterest)
        directoryItem = listing(iDirectoryItem);
        filePath = fullfile(directoryItem.folder, directoryItem.name);
        filePaths{iFilePathCounter} = filePath;
        iFilePathCounter = iFilePathCounter + 1;
    end
end


end