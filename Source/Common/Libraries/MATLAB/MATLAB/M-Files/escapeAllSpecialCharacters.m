function literalText = escapeAllSpecialCharacters(text)
% Escapes all special characters in a text string to produce a literal text
% string.
%
% https://stackoverflow.com/questions/15486611/how-to-add-before-all-special-characters-in-matlab


%%

literalText = regexprep(text,'([[\]{}()=''.(),;:%%{%}!@])','\\$1');


end