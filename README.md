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
math in order to get the value you'd like. Further work on the Currency class can overcome those limitations.
  
What Every Computer Scientist Should Know About Floating-Point Arithmetic
https://docs.oracle.com/cd/E19957-01/806-3568/ncg_goldberg.html

The program output looks like this. If you want to, you can mess with the formula to get a more or less dramatic rate of error.
```
iformula: a + (b * b + c)
a = 1000.00
b =   23.23
c =   45.20
             decimal            currency               float              double           diff(C-m)           diff(f-m)           diff(d-m)
           1584.8329           1584.8329            1584.833           1584.8329              0.0000            -0.00001              0.0000
           2169.6658           2169.6658            2169.666           2169.6658              0.0000            -0.00003              0.0000
           2754.4987           2754.4987            2754.499           2754.4987              0.0000            -0.00016              0.0000
           3339.3316           3339.3316            3339.332           3339.3316              0.0000            -0.00006     0.0000000000004
           3924.1645           3924.1645            3924.165           3924.1645              0.0000             0.00005     0.0000000000008
           4508.9974           4508.9974            4508.998           4508.9974              0.0000             0.00016     0.0000000000011
           5093.8303           5093.8303            5093.831           5093.8303              0.0000             0.00027     0.0000000000014
           5678.6632           5678.6632            5678.664           5678.6632              0.0000             0.00037     0.0000000000018
           6263.4961           6263.4961            6263.497           6263.4961              0.0000             0.00048     0.0000000000021
           6848.3290            6848.329             6848.33            6848.329              0.0000             0.00059     0.0000000000025
           7433.1619           7433.1619            7433.163           7433.1619              0.0000              0.0007     0.0000000000028
           8017.9948           8017.9948            8017.996           8017.9948              0.0000             0.00081     0.0000000000031
           8602.8277           8602.8277            8602.828           8602.8277              0.0000              0.0003     0.0000000000035
           9187.6606           9187.6606            9187.661           9187.6606              0.0000              0.0004     0.0000000000029
           9772.4935           9772.4935            9772.494           9772.4935              0.0000              0.0005     0.0000000000023
```
