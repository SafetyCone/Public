% 2D
syms e1 e2 a1 a2;
A = [[e1, e2]; [a1, a2]];
detA = det(A);

e1Value = subs(detA, [e1, e2], [1, 0]);
e2Value = subs(detA, [e1, e2], [0, 1]);

newOrthogV = [e1Value, e2Value];

shouldBeZero = newOrthogV * [a1; a2]; % Yep!

% 3D
syms e1 e2 e3 a1 a2 a3 b1 b2 b3;
A = [[e1, e2, e3]; [a1, a2, a3]; [b1, b2, b3]];

detA = det(A);

e1Value = subs(detA, [e1, e2, e3], [1, 0, 0]);
e2Value = subs(detA, [e1, e2, e3], [0, 1, 0]);
e3Value = subs(detA, [e1, e2, e3], [0, 0, 1]);

newOrthogV = [e1Value, e2Value, e3Value];

shouldBeZero = newOrthogV * [a1; a2; a3];
simplify(shouldBeZero); % Yep!

% 4D
syms e1 e2 e3 e4 a1 a2 a3 a4 b1 b2 b3 b4 c1 c2 c3 c4;
A = [[e1, e2, e3, e4]; [a1, a2, a3, a4]; [b1, b2, b3, b4]; [c1, c2, c3, c4]];

detA = det(A);

e1Value = subs(detA, [e1, e2, e3, e4], [1, 0, 0, 0]);
e2Value = subs(detA, [e1, e2, e3, e4], [0, 1, 0, 0]);
e3Value = subs(detA, [e1, e2, e3, e4], [0, 0, 1, 0]);
e4Value = subs(detA, [e1, e2, e3, e4], [0, 0, 0, 1]);

newOrthogV = [e1Value, e2Value, e3Value, e4Value];

shouldBeZero = newOrthogV * [a1; a2; a3; a4];
simplify(shouldBeZero); % Yep!

% 4D, more computational.
syms e1 e2 e3 e4 a1 a2 a3 a4 b1 b2 b3 b4 c1 c2 c3 c4;
A = [[e1, e2, e3, e4]; [a1, a2, a3, a4]; [b1, b2, b3, b4]; [c1, c2, c3, c4]];

A1 = A(2:end, [2, 3, 4]);
A2 = A(2:end, [1, 3, 4]);
A3 = A(2:end, [1, 2, 4]);
A4 = A(2:end, [1, 2, 3]);

newOrthogV = [det(A1), -det(A2), det(A3), -det(A4)];

shouldBeZero = newOrthogV * [a1; a2; a3; a4];
simplify(shouldBeZero); % Yep!









