%% Setup

imageDirectoryPath = 'E:\Organizations\Minex\Data\Images\Camera Calibration\Checkerboard\iPhone 6\Session 2';
exampleImagePath = 'E:\Organizations\Minex\Data\Images\Princeton\Campus\IMG_0012.JPG';
forceCheckerboardRedetection = false;
worldUnits = 'millimeters';
boardSize = [7, 10];
squareSize = 23; % In world units.
imageFileExtensions = { 'jpg', 'png' };


%% Get all image file paths in directory.

imageFilePaths = filePathsByExtensions(imageDirectoryPath, imageFileExtensions);


%% Detect checkerboards in images.
% This step takes a long time (~30 seconds per image, 20 images).

% Determine if the results already exist.
dataDirectoryPath = fullfile(projectDirectoryPath, 'Data');
if(not(directoryExists(dataDirectoryPath)))
    mkdir(dataDirectoryPath);
end

imagePointsDataFilePath = fullfile(dataDirectoryPath, 'imagePoints.mat');
imagesUsedLogicalsDataFilePath = fullfile(dataDirectoryPath, 'imagesUsedLogicals.mat');

if(not(fileExists(imagePointsDataFilePath)) || forceCheckerboardRedetection)
    % Recompute.
    [imagePoints, imagesUsedLogicals] = detectCheckerboardImagePoints(imageFilePaths);
    
    save(imagePointsDataFilePath, 'imagePoints');
    save(imagesUsedLogicalsDataFilePath, 'imagesUsedLogicals');
else
    load(imagePointsDataFilePath);
    load(imagesUsedLogicalsDataFilePath);
end


%% Calibrate the camera.

imageFilePathsUsed = imageFilePaths(imagesUsedLogicals);

% Set level of camera instrinsics model detail.
estimateSkew = true;
estimateTangentialDistortion = false;
numRadialDistortionCoefficients = 2; % 2 or 3.

% Read the first image to obtain image size
originalImage = imread(imageFilePathsUsed{1});
[mrows, ncols, ~] = size(originalImage);

boardConfiguration.worldUnits = worldUnits;
boardConfiguration.boardSize = boardSize;
boardConfiguration.squareSize = squareSize;

estimationParameters.estimateSkew = estimateSkew;
estimationParameters.estimateTangentialDistortion = estimateTangentialDistortion;
estimationParameters.numRadialDistortionCoefficients = numRadialDistortionCoefficients;

imageSize = [mrows, ncols];

% [cameraParams, imagesUsed, estimationErrors] = calibrateCamera(imagePoints, worldUnits, boardSize, squareSize, estimateSkew, estimateTangentialDistortion, numRadialDistortionCoefficients, mrows, ncols);
[cameraParams, imagesUsed, estimationErrors] = calibrateCameraStructs(imagePoints, boardConfiguration, estimationParameters, imageSize);


%% Display.

% View reprojection errors
h1=figure; showReprojectionErrors(cameraParams);

% Visualize pattern locations
h2=figure; showExtrinsics(cameraParams, 'CameraCentric');

% Display parameter estimation errors
displayErrors(estimationErrors, cameraParams);

% For example, you can use the calibration data to remove effects of lens distortion.
undistortedImage = undistortImage(originalImage, cameraParams);
figure; imshow(undistortedImage);

exampleImage = imread(exampleImagePath);
undistortedExampleImage = undistortImage(exampleImage, cameraParams);
figure; imshow(undistortedExampleImage);


%% Save.

estimateSkewSuffix = ''; %#ok<NASGU>
if(estimateSkew)
    estimateSkewSuffix = 'Skew'; 
end

estimateTangentialDistortionSuffix = '';
if(estimateTangentialDistortion)
    estimateTangentialDistortionSuffix = 'Tang';  %#ok<UNRCH>
end

parametersFilePath = fullfile(imageDirectoryPath, strcat('Parameters', num2str(numRadialDistortionCoefficients), estimateSkewSuffix, estimateTangentialDistortionSuffix, '.mat'));
save(parametersFilePath, 'cameraParams');

errorsFilePath = fullfile(imageDirectoryPath, strcat('Errors', num2str(numRadialDistortionCoefficients), estimateSkewSuffix, estimateTangentialDistortionSuffix, '.mat'));
save(errorsFilePath, 'estimationErrors');































