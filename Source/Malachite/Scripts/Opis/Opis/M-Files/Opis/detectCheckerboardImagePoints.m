function [imagePoints, imagesUsedLogicals] = detectCheckerboardImagePoints(imageFilePaths)
% detectCheckerboardImagePoints - Given a list of image file paths, call
% the vision toolbox's detectCheckerboardPoints function.
%
% This takes 30 seconds per image. Thus the image point results should be
% saved after the computation.
%
% The function uses a waitbar.


%%

nImages = numel(imageFilePaths);
imagePointsCell = cell(nImages, 1);
imagesUsedCounter = 0;
imagesUsedLogicals = false(nImages, 1);
hWaitbar = waitbar(0, '');
for iImagePath = 1:nImages
    imageFilePath = imageFilePaths{iImagePath};
    
    [~, fileName] = fileparts(imageFilePath);
    message = sprintf('Processing image %d/%d: %s', iImagePath, nImages, fileName);
    literalMessage = escapeAllCharacters(message);
    waitbar(iImagePath / nImages, hWaitbar, literalMessage);
    
    [curImagePoints, ~, imagesUsed] = detectCheckerboardPoints(imageFilePath); % Ignore the boardsize for now.
    if(1 == sum(imagesUsed))
        imagesUsedLogicals(iImagePath, 1) = true;
        imagesUsedCounter = imagesUsedCounter + 1;
        imagePointsCell{imagesUsedCounter} = curImagePoints;
    end
end
close(hWaitbar);

imagePointSize = size(imagePointsCell{1});
imagePoints = zeros([imagePointSize(:); nImages]');
for iImage = 1:imagesUsedCounter
    imagePoints(:, :, iImage) = imagePointsCell{iImage};
end


end