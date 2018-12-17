The following code demonstrates how a floating point number rounding error can
impact arithmetic operations in C#. Note that the float data-type has a rounding
error immediately, where the double data-type gets through 3 iterations before
the error shows up. Clearly, repeated operations exacerbates the problem. After
15 million iterations the calculations will be off by several significant digits.
In the C# programming language, the decimal data-type is the recommended data-type
for currency operations. This code uses decimal as the baseline of correctness.
decimal is so large/wide that it is unreasonable to expect an issue in calculations
that we might commonly experience in non-scientific computing. The range of the
decimal data-type is ±1.0 x 10^-28 to ±7.9228 x 10^28. 

The Currency Class included here shows one technique of using a fixed point 
representation. Using an adjustment value of 1,000,000 extends the capability of
the Currency class to operate well beyond the limits of the double data-type.
A larger adjustment value will result in even greater range. However, parsing 
limitations in this implementation create an artificial upper and lower bound
for initialization. So, if you bump into those limits you may need to do some
math in order to get the value you'd like. Further work on the Currency class can
overcome those limitations.
  
What Every Computer Scientist Should Know About Floating-Point Arithmetic
https://docs.oracle.com/cd/E19957-01/806-3568/ncg_goldberg.html
