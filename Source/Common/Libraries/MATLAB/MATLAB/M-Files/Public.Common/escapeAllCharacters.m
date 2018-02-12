function literalText = escapeAllCharacters(text)
% Escapes all special characters in a text string to produce a literal text
% string.
%
% https://stackoverflow.com/questions/15486611/how-to-add-before-all-special-characters-in-matlab


%%

literalText = regexprep(text, '([[\]{}()=''.(),\;:%%{%}!@])|_', '\\$0');


end