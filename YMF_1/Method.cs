namespace YMF_1;

public class Method
{
// void block_relaxation(double omega, int block)
//    {
//       int num = size_ / block;
//       for (int i = 0; i < num; i++)
//       {
//          std::vector<type> Ri = Ri_(block, i);
//          for (size_t i = 0; i < block; i++)
//          {
//             Ri[i] *= omega;
//          }
//          std::vector<type> solution = LU_SOL(i, block, Ri);
//          for (int j = 0; j < block; j++)
//          {
//             x_[i * block + j] += solution[j];
//          }
//       }
//       int k =0;
//    };
//
//    type error_block()
//    {
//       type  R1 = 0.0, R2 = 0.0;
//       for (int i = 0; i < size_; i++)
//       {
//          R1 += (f_[i] - sum_block(i)) * (f_[i] - sum_block(i));
//          R2 += f_[i] * f_[i];
//       }
//       return sqrt(R1 / R2);
//    };
//
//    sum sum_block(int i)
//    {
//       sum sum = 0.0;
//       if (i == 0)
//       {
//          sum += org_di[0] * x_[i] + org_u[0] * x_[i + 1];
//       }
//
//       if (i >= 0 && i <= size_ - m_ - 3)
//       {
//          sum += A_[1][i] * x_[i + m_ + 2];
//       }
//
//       if (i >= 0 && i <= size_ - m_ - 4)
//       {
//          sum += A_[0][i] * x_[i + m_ + 3];
//       }
//
//       if (i <= size_ - 1 && i >= m_ + 2)
//       {
//          sum += A_[5][i - m_ - 2] * x_[i - m_ - 2];
//       }
//
//       if (i <= size_ - 1 && i >= m_ + 3)
//       {
//          sum += A_[6][i - m_ - 3] * x_[i - m_ - 3];
//       }
//
//       if (i > 0 && i < size_ - 1)
//       {
//          sum += org_di[i] * x_[i] + org_u[i] * x_[i + 1] +org_l[i - 1] * x_[i - 1];
//       }
//
//       if (i == size_ - 1)
//       {
//          sum += org_di[i] * x_[i] + org_l[i - 1] * x_[i - 1];
//       }
//
//       return sum;
//    }
//
//    void LU(int block)
//    {
//       org_di = A_[3];
//       org_u = A_[2];
//       org_l = A_[4];
//       int num = size_ / block;
//       int u = 2;
//       int l = 4;
//
//       for (int i = 0; i < num; i++)
//       {
//          for (int j = i * block; j < (i + 1) * block; j++)
//          {
//             if (j - 1 >= i * block)
//             {
//                A_[3][j] -= A_[l][j-1] * A_[u][j - 1];
//             }
//             if (j + 1 < (i + 1) * block)
//             {
//                A_[u][j] /= A_[3][j];
//             }
//          }
//       }
//    };
//
//    std::vector<type> LU_SOL(int i, int block, std::vector<type>& Ri)
//    {
//       std::vector<type> y(block);
//       int u = 2;
//       int l = 4;
//       y[0] = Ri[0] / A_[3][i * block];
//       for (int counter = 1; counter < block; counter++)
//       {
//          int new_offset = i * block + counter;
//          y[counter] = (Ri[counter] - A_[l][i * block + counter-1] * y[counter - 1]) / A_[3][i * block + counter];
//       }
//       std::vector<type> x(block);
//       x[block - 1] = y[block - 1];
//       for (int counter = block - 2; counter >= 0; counter--)
//       {
//          x[counter] = y[counter] - A_[u][i * block + counter] * x[counter + 1];
//       }
//       return x;
//    }
//
//    std::vector<type> Ri_(int block, int i)
//    {
//       int num = size_ / block;
//       std::vector <type> Ri(block);
//       for (int n = 0; n < block; n++)
//       {
//          Ri[n] = f_[i * block + n];
//       }
//       for (int j = 0; j < num; j++)
//       { 
//          int start_col = j * block;
//          int end_col = (j + 1) * block;
//          int start_row = i * block;
//          int end_row = (i + 1) * block;
//          for (int n = start_row; n < end_row; n++)
//          {
//             sum sum = 0.0;
//             for (int m = start_col; m < end_col; m++)
//             {
//                sum += A_r(n,m)*x_[m];
//             }
//             Ri[n % block] -= sum;
//          }
//       }
//       return Ri;
//    };
//
//    type A_r(int n, int m)
//    {
//       if (m == n)
//          return org_di[m];
//       if (m == n + 1)
//          return org_u[n];
//       if (m == n - 1)
//          return org_l[m];
//       if (m == n + 2 + m_)
//          return A_[1][n];
//       if (m == n + 3 + m_)
//          return A_[0][n];
//       if (m == n - 3 - m_)
//          return A_[6][m];
//       if (m == n - 2 - m_)
//          return A_[5][m];
//       return 0;
//    };
}