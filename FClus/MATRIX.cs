
using System;

namespace FClus
{
	public class MATRIX
	{
		private int m_iRows;
		private int m_iCols;
		Fraction[,] m_iElement;
		

		/// <summary>
		/// Constructors
		/// </summary>
		public MATRIX(Fraction[,] elements)
		{
			m_iElement=elements;
			m_iRows=elements.GetLength(0);
			m_iCols=elements.GetLength(1);
		}
		
		public MATRIX(int[,] elements)
		{
			m_iRows=elements.GetLength(0);
			m_iCols=elements.GetLength(1);;
			m_iElement=new Fraction[m_iRows,m_iCols];
			for(int i=0;i<elements.GetLength(0);i++)
			{
				for(int j=0;j<elements.GetLength(1);j++)
				{
					this[i,j]=new Fraction( elements[i,j] );
				}
			}
		}

		public MATRIX(double[,] elements)
		{
			m_iRows=elements.GetLength(0);
			m_iCols=elements.GetLength(1);;
			m_iElement=new Fraction[m_iRows,m_iCols];
			for(int i=0;i<elements.GetLength(0);i++)
			{
				for(int j=0;j<elements.GetLength(1);j++)
				{
					this[i,j]=Fraction.ConvertToFraction( elements[i,j] );
				}
			}
		}

        public void initMatrix()
        {
            for (int x = 0; x < Rows; x++)
                for (int y = 0; y < Cols; y++)
                    this[x, y] = new Fraction();
        }

        public MATRIX(int iRows, int iCols)
		{
			m_iRows=iRows;
			m_iCols=iCols;
			m_iElement=new Fraction[iRows,iCols];
		}
	
		/// <summary>
		/// Properites
		/// </summary>
		public int Rows
		{
			get	{ return m_iRows;	}
		}	
	
		public int Cols
		{
			get	{ return m_iCols;	}
		}	

        /// <summary>
        /// return elements
        /// </summary>
        public Fraction[,] Elements
        {
            get { return m_iElement; }
        }
		/// <summary>
		/// Indexer
		/// </summary>
		public Fraction this[int iRow, int iCol]		// matrix's index starts at 0,0
		{
			get	{	return GetElement(iRow,iCol);	}
			set	{	SetElement(iRow,iCol,value);	}
		}

		/// <summary>
		/// Internal functions for getting/setting values
		/// </summary>
		private Fraction GetElement(int iRow, int iCol)
		{
				if ( iRow<0 || iRow>Rows-1 || iCol<0 || iCol>Cols-1 )
					throw new MatrixException("Invalid index specified");
				return m_iElement[iRow,iCol];
		}
	
		private void SetElement(int iRow, int iCol, Fraction value)
		{
				if ( iRow<0 || iRow>Rows-1 || iCol<0 || iCol>Cols-1 )
					throw new MatrixException("Invalid index specified");
				m_iElement[iRow,iCol]=value.Duplicate();
		}

		
		/// <summary>
		/// The function returns the current Matrix object as a string
		/// </summary>
		public string ConvertToString()
		{
			return (MATRIX.ConvertToString(this));
		}
		
		/// <summary>
		/// The function takes a Matrix object and returns it as a string
		/// </summary>
		public static string ConvertToString(MATRIX matrix)
		{
			string str="";
			for (int i=0;i<matrix.Rows;i++)
			{
				for (int j=0;j<matrix.Cols;j++)
					str+=matrix[i,j].ConvertToString()+"\t";
				str+="\n";
			}
			return str;
		}
		
		/// <summary>
		/// The function return the Minor of element[Row,Col] of a Matrix object 
		/// </summary>
		public static MATRIX Minor(MATRIX matrix, int iRow, int iCol)
		{
			MATRIX minor=new MATRIX(matrix.Rows-1, matrix.Cols-1);
			int m=0,n=0;
			for (int i=0;i<matrix.Rows;i++)
			{
				if (i==iRow)
					continue;
				n=0;
				for (int j=0;j<matrix.Cols;j++)
				{
					if (j==iCol) 
						continue;
					minor[m,n]=matrix[i,j];
					n++;	
				}
				m++;
			}
			return minor;
		}
			
			
		
		/// <summary>
		/// The function returns the determinent of a Matrix object as Fraction
		/// </summary>
		public static Fraction Determinent(MATRIX matrix)
		{
			Fraction det=new Fraction(0);
			if (matrix.Rows!=matrix.Cols)
				throw new MatrixException("Determinent of a non-square matrix doesn't exist");
			if (matrix.Rows==1)
				return matrix[0,0];
			for (int j=0;j<matrix.Cols;j++)
				det+=(matrix[0,j]*Determinent(MATRIX.Minor(matrix, 0,j))*(int)System.Math.Pow(-1,0+j));
			return det;
		}
	
		/// <summary>
		/// The function returns the determinent of the current Matrix object as Fraction
		/// </summary>
		public Fraction Determinent()
		{
			return Determinent(this);
		}
		
		/// <summary>
		/// The function multiplies the given row of the current matrix object by a Fraction 
		/// </summary>
		public void MultiplyRow(int iRow, Fraction frac)
		{
			for (int j=0;j<this.Cols;j++)
			{
				this[iRow,j]*=frac;
				Fraction.ReduceFraction(this[iRow,j]);
			}
		}

		/// <summary>
		/// The function multiplies the given row of the current matrix object by an integer
		/// </summary>
		public void MultiplyRow(int iRow, int iNo)
		{
			this.MultiplyRow(iRow, new Fraction(iNo));
		}

		/// <summary>
		/// The function multiplies the given row of the current matrix object by a double
		/// </summary>
		public void MultiplyRow(int iRow, double dbl)
		{
			this.MultiplyRow(iRow, Fraction.ConvertToFraction(dbl));
		}
		
		/// <summary>
		/// The function adds two rows for current matrix object
		/// It performs the following calculation:
		/// iTargetRow = iTargetRow + iMultiple*iSecondRow
		/// </summary>
		public void AddRow(int iTargetRow, int iSecondRow, Fraction iMultiple)
		{
			for (int j=0;j<this.Cols;j++)
				this[iTargetRow,j]+=(this[iSecondRow,j]*iMultiple);
		}

		/// <summary>
		/// The function interchanges two rows of the current matrix object
		/// </summary>
		public void InterchangeRow(int iRow1, int iRow2)
		{
			for (int j=0;j<this.Cols;j++)
			{
				Fraction temp=this[iRow1,j];
				this[iRow1,j]=this[iRow2,j];
				this[iRow2,j]=temp;
			}
		}
		
		/// <summary>
		/// The function concatenates the two given matrices column-wise
		/// </summary>
		public static MATRIX Concatenate(MATRIX matrix1, MATRIX matrix2)
		{
			if (matrix1.Rows!=matrix2.Rows)
				throw new MatrixException("Concatenation not possible");
			MATRIX matrix=new MATRIX(matrix1.Rows, matrix1.Cols+matrix2.Cols );
			for (int i=0;i<matrix.Rows;i++)
				for (int j=0;j<matrix.Cols;j++)
				{
					if ( j<matrix1.Cols )
						matrix[i,j]=matrix1[i,j];
					else 
						matrix[i,j]=matrix2[i,j-matrix1.Cols];
				}	
			return matrix;
		}

		/// <summary>
		/// The function returns the Echelon form of a given matrix
		/// </summary>
		public static MATRIX EchelonForm(MATRIX matrix)
		{
			try
			{
				MATRIX EchelonMatrix=matrix.Duplicate();
				for (int i=0;i<matrix.Rows;i++)
				{	
					if (EchelonMatrix[i,i]==0)	// if diagonal entry is zero, 
						for (int j=i+1;j<EchelonMatrix.Rows;j++)
							if (EchelonMatrix[j,i]!=0)	 //check if some below entry is non-zero
								EchelonMatrix.InterchangeRow(i,j);	// then interchange the two rows
					if (EchelonMatrix[i,i]==0)	// if not found any non-zero diagonal entry
						continue;	// increment i;
					if ( EchelonMatrix[i,i]!=1)	// if diagonal entry is not 1 , 	
						for (int j=i+1;j<EchelonMatrix.Rows;j++)
							if (EchelonMatrix[j,i]==1)	 //check if some below entry is 1
								EchelonMatrix.InterchangeRow(i,j);	// then interchange the two rows
					EchelonMatrix.MultiplyRow(i, Fraction.Inverse(EchelonMatrix[i,i]));
					for (int j=i+1;j<EchelonMatrix.Rows;j++)
						EchelonMatrix.AddRow( j, i, -EchelonMatrix[j,i]);
				}
				return EchelonMatrix;
			}
			catch(Exception)
			{
				throw new MatrixException("Matrix can not be reduced to Echelon form");
			}
		}

		/// <summary>
		/// The function returns the Echelon form of the current matrix
		/// </summary>
		public MATRIX EchelonForm()
		{
			return EchelonForm(this);
		}

		/// <summary>
		/// The function returns the reduced echelon form of a given matrix
		/// </summary>
		public static MATRIX ReducedEchelonForm(MATRIX matrix)
		{
			try
			{
				MATRIX ReducedEchelonMatrix=matrix.Duplicate();
				for (int i=0;i<matrix.Rows;i++)
				{	
					if (ReducedEchelonMatrix[i,i]==0)	// if diagonal entry is zero, 
						for (int j=i+1;j<ReducedEchelonMatrix.Rows;j++)
							if (ReducedEchelonMatrix[j,i]!=0)	 //check if some below entry is non-zero
								ReducedEchelonMatrix.InterchangeRow(i,j);	// then interchange the two rows
					if (ReducedEchelonMatrix[i,i]==0)	// if not found any non-zero diagonal entry
						continue;	// increment i;
					if ( ReducedEchelonMatrix[i,i]!=1)	// if diagonal entry is not 1 , 	
						for (int j=i+1;j<ReducedEchelonMatrix.Rows;j++)
							if ( ReducedEchelonMatrix[j,i]==1 )	 //check if some below entry is 1
								ReducedEchelonMatrix.InterchangeRow(i,j);	// then interchange the two rows
					ReducedEchelonMatrix.MultiplyRow(i, Fraction.Inverse(ReducedEchelonMatrix[i,i]));
					for (int j=i+1;j<ReducedEchelonMatrix.Rows;j++)
						ReducedEchelonMatrix.AddRow( j, i, -ReducedEchelonMatrix[j,i]);
					for (int j=i-1;j>=0;j--)
						ReducedEchelonMatrix.AddRow( j, i, -ReducedEchelonMatrix[j,i]);
				}
				return ReducedEchelonMatrix;
			}
			catch(Exception)
			{
				throw new MatrixException("Matrix can not be reduced to Echelon form");
			}
		}

		/// <summary>
		/// The function returns the reduced echelon form of the current matrix
		/// </summary>
		public MATRIX ReducedEchelonForm()
		{
			return ReducedEchelonForm(this);
		}

		/// <summary>
		/// The function returns the inverse of a given matrix
		/// </summary>
		public static MATRIX Inverse(MATRIX matrix)
		{
			if ( Determinent(matrix)==0 )
				throw new MatrixException("Inverse of a singular matrix is not possible");
			return ( Adjoint(matrix)/Determinent(matrix) );
		}
		
		/// <summary>
		/// The function returns the inverse of the current matrix
		/// </summary>
		public MATRIX Inverse()
		{
			return Inverse(this);
		}

		/// <summary>
		/// The function returns the adjoint of a given matrix
		/// </summary>
		public static MATRIX Adjoint(MATRIX matrix)
		{
			if (matrix.Rows!=matrix.Cols)
				throw new MatrixException("Adjoint of a non-square matrix does not exists");
			MATRIX AdjointMatrix=new MATRIX(matrix.Rows, matrix.Cols);
			for (int i=0;i<matrix.Rows;i++)
				for (int j=0;j<matrix.Cols;j++)
					AdjointMatrix[i,j]=Math.Pow(-1,i+j)*Determinent(Minor(matrix, i,j));
			AdjointMatrix=Transpose(AdjointMatrix);
			return AdjointMatrix;
		}
		
		/// <summary>
		/// The function returns the adjoint of the current matrix
		/// </summary>
		public MATRIX Adjoint()
		{
			return Adjoint(this);
		}
		
		/// <summary>
		/// The function returns the transpose of a given matrix
		/// </summary>
		public static MATRIX Transpose(MATRIX matrix)
		{
			MATRIX TransposeMatrix=new MATRIX(matrix.Cols, matrix.Rows);
			for (int i=0;i<TransposeMatrix.Rows;i++)
				for (int j=0;j<TransposeMatrix.Cols;j++)
					TransposeMatrix[i,j]=matrix[j,i];
			return TransposeMatrix;
		}
		
		/// <summary>
		/// The function returns the transpose of the current matrix
		/// </summary>
		public MATRIX Transpose()
		{
			return Transpose(this);
		}
		
		/// <summary>
		/// The function duplicates the current Matrix object
		/// </summary>
		public MATRIX Duplicate()
		{
			MATRIX matrix=new MATRIX(Rows,Cols);
			for (int i=0;i<Rows;i++)
				for (int j=0;j<Cols;j++)
					matrix[i,j]=this[i,j];
			return matrix;
		}
	
		/// <summary>
		/// The function returns a Scalar Matrix of dimension ( Row x Col ) and scalar K
		/// </summary>
		public static MATRIX ScalarMatrix(int iRows, int iCols, int K)
		{
			Fraction zero=new Fraction(0);
			Fraction scalar=new Fraction(K);
			MATRIX matrix=new MATRIX(iRows,iCols);
			for (int i=0;i<iRows;i++)
				for (int j=0;j<iCols;j++)
				{
					if (i==j)
						matrix[i,j]=scalar;
					else
						matrix[i,j]=zero;
				}
			return matrix;
		}

		/// <summary>
		/// The function returns an identity matrix of dimensions ( Row x Col )
		/// </summary>
		public static MATRIX IdentityMatrix(int iRows, int iCols)
		{
			return ScalarMatrix(iRows, iCols, 1);
		}

		/// <summary>
		/// The function returns a Unit Matrix of dimension ( Row x Col )
		/// </summary>
		public static MATRIX UnitMatrix(int iRows, int iCols)
{
			Fraction temp=new Fraction(1);
			MATRIX matrix=new MATRIX(iRows,iCols);
			for (int i=0;i<iRows;i++)
				for (int j=0;j<iCols;j++)
					matrix[i,j]=temp;
			return matrix;
		}
		
		/// <summary>
		/// The function returns a Null Matrix of dimension ( Row x Col )
		/// </summary>
		public static MATRIX NullMatrix(int iRows, int iCols)
		{
			Fraction temp=new Fraction(0);
			MATRIX matrix=new MATRIX(iRows,iCols);
			for (int i=0;i<iRows;i++)
				for (int j=0;j<iCols;j++)
					matrix[i,j]=temp;
			return matrix;
		}
		
		/// <summary>
		/// Operators for the Matrix object
		/// includes -(unary), and binary opertors such as +,-,*,/
		/// </summary>
		public static MATRIX operator -(MATRIX matrix)
		{	return MATRIX.Negate(matrix);	}
		
		public static MATRIX operator +(MATRIX matrix1, MATRIX matrix2)
		{	return MATRIX.Add(matrix1, matrix2);	}
		
		public static MATRIX operator -(MATRIX matrix1, MATRIX matrix2)
		{	return MATRIX.Add(matrix1, -matrix2);	}
		
		public static MATRIX operator *(MATRIX matrix1, MATRIX matrix2)
		{	return MATRIX.Multiply(matrix1, matrix2);	}
	
		public static MATRIX operator *(MATRIX matrix1, int iNo)
		{	return MATRIX.Multiply(matrix1, iNo);	}
	
		public static MATRIX operator *(MATRIX matrix1, double dbl)
		{	return MATRIX.Multiply(matrix1, Fraction.ConvertToFraction(dbl));	}
	
		public static MATRIX operator *(MATRIX matrix1, Fraction frac)
		{	return MATRIX.Multiply(matrix1, frac);	}
	
		public static MATRIX operator *(int iNo, MATRIX matrix1)
		{	return MATRIX.Multiply(matrix1, iNo);	}
	
		public static MATRIX operator *(double dbl, MATRIX matrix1)
		{	return MATRIX.Multiply(matrix1, Fraction.ConvertToFraction(dbl));	}

		public static MATRIX operator *(Fraction frac, MATRIX matrix1)
		{	return MATRIX.Multiply(matrix1, frac);	}
	
		public static MATRIX operator /(MATRIX matrix1, int iNo)
		{	return MATRIX.Multiply(matrix1, Fraction.Inverse(new Fraction(iNo)));	}
		
		public static MATRIX operator /(MATRIX matrix1, double dbl)
		{	return MATRIX.Multiply(matrix1, Fraction.Inverse(Fraction.ConvertToFraction(dbl)));	}
		
		public static MATRIX operator /(MATRIX matrix1, Fraction frac)
		{	return MATRIX.Multiply(matrix1, Fraction.Inverse(frac));	}
	
		
		/// <summary>
		/// Internal Fucntions for the above operators
		/// </summary>
		private static MATRIX Negate(MATRIX matrix)
		{
			return MATRIX.Multiply(matrix,-1);
		}
		
		private static MATRIX Add(MATRIX matrix1, MATRIX matrix2)
		{
			if (matrix1.Rows!=matrix2.Rows || matrix1.Cols!=matrix2.Cols)
				throw new MatrixException("Operation not possible");
			MATRIX result=new MATRIX(matrix1.Rows, matrix1.Cols);
			for (int i=0;i<result.Rows;i++)
				for (int j=0;j<result.Cols;j++)
					result[i,j]=matrix1[i,j]+matrix2[i,j];
			return result;
		}
		
		private static MATRIX Multiply(MATRIX matrix1, MATRIX matrix2)
		{
			if ( matrix1.Cols!=matrix2.Rows )
				throw new MatrixException("Operation not possible");
			MATRIX result=MATRIX.NullMatrix(matrix1.Rows,matrix2.Cols);
			for (int i=0;i<result.Rows;i++)
				for (int j=0;j<result.Cols;j++)
					for (int k=0;k<matrix1.Cols;k++)
						result[i,j]+=matrix1[i,k]*matrix2[k,j];
			return result;						
		}
		
		private static MATRIX Multiply(MATRIX matrix, int iNo)
		{
			MATRIX result=new MATRIX(matrix.Rows,matrix.Cols);
			for (int i=0;i<matrix.Rows;i++)
				for (int j=0;j<matrix.Cols;j++)
					result[i,j]=matrix[i,j]*iNo;
			return result;
		}
		
		private static MATRIX Multiply(MATRIX matrix, Fraction frac)
		{
			MATRIX result=new MATRIX(matrix.Rows,matrix.Cols);
			for (int i=0;i<matrix.Rows;i++)
				for (int j=0;j<matrix.Cols;j++)
					result[i,j]=matrix[i,j]*frac;
			return result;
		}
	
	}	//end class Matrix
	
	/// <summary>
	/// Exception class for Matrix class, derived from System.Exception
	/// </summary>
	public class MatrixException : Exception
	{
		public MatrixException() : base()
		{}
	
		public MatrixException(string Message) : base(Message)
		{}
		
		public MatrixException(string Message, Exception InnerException) : base(Message, InnerException)
		{}
	}	// end class MatrixException

	/// <summary>
	/// class name: Application
	/// The class demonstrates the Matrix class
	/// </summary>
/*	public class MatrixApplication
	{
		public static void Main()
		{
			try
			{
				Console.WriteLine("Press 'm' for Matrix demo, 'e' for Equation Solver demo");
				if ( Console.ReadLine().ToLower()=="e" )
					EquationSolver();
				else					
					MatrixDemo();
				Console.WriteLine("\n\nPress any key to exit");
			}	//end try
			catch (MatrixException exp)
			{
				Console.WriteLine("\nInternal Matrix Error: " + exp.Message);
			}
			catch (FractionException exp)
			{
				Console.WriteLine("\nInternal Fraction Error: " + exp.Message);
			}
			catch (Exception exp)
			{
				Console.WriteLine("\nSystem Error: " + exp.Message);
			}
			Console.ReadLine();
		}
		
		public static void MatrixDemo()
		{
			Console.WriteLine("Enter no. of Rows: ");
			int iRows=Convert.ToInt32(Console.ReadLine());
			Console.WriteLine("Enter no. of Cols: ");
			int iCols=Convert.ToInt32(Console.ReadLine());
			
			MATRIX m1=new MATRIX(iRows,iCols);
			MATRIX m2=new MATRIX(iRows,iCols);
			System.Random rnd=new System.Random();
			for ( int i=0;i<iRows;i++)
			{
				for ( int j=0;j<iCols;j++)
				{
					m1[i,j]=new Fraction( Convert.ToString( (rnd.Next()%10)-5 ) );
//					m1[i,j]=new Fraction(Console.ReadLine());
					m2[i,j]=new Fraction( Convert.ToString( (rnd.Next()%5.5)-2 ) );
				}
			}
			Console.WriteLine("\n\nLet the two matrices be:");
			Console.WriteLine( "\nm1\n"+ m1.ConvertToString() );
			Console.WriteLine( "\nm2\n"+ m2.ConvertToString() );
			Console.ReadLine();
			Console.WriteLine("\nm1+m2=\n"+ (m1+m2).ConvertToString());
			Console.WriteLine("\nm1*m2=\n");
			Console.WriteLine( (m1*m2).ConvertToString());
			Console.ReadLine();
			Console.WriteLine( "\nEchelon Form for m1:\n"+ m1.EchelonForm().ConvertToString() );
			Console.WriteLine( "\nReduced Echelon Form for m1:\n"+ m1.ReducedEchelonForm().ConvertToString() );
			Console.ReadLine();
			Console.WriteLine("\nDeterminent of m1="+ m1.Determinent().ConvertToString());
			Console.WriteLine("\nTranspose m1:\n"+ m1.Transpose().ConvertToString());
			Console.WriteLine("\nInverse of m1:\n "+ m1.Inverse().ConvertToString() );
		}	//end MatrixDemo()
		
		public static void EquationSolver()
		{
			Console.Write("Enter no. of variables: ");
			int iVarNo=Convert.ToInt32( Console.ReadLine() );
			string str;
			MATRIX mat=new MATRIX(iVarNo,iVarNo+1);
			Console.Write("\nEnter value of coefficents and constants (can be an integer(e.g 3), a floating point no(e.g. 12.25), or a fraction(e.g. 15/19)");
			int i,cnt;
			for (i=0;i<iVarNo+1;i++)
			{
				cnt=1;
				for (int k=0;k<i;k++)
				{
					Console.Write("\n"+"Equation "+(k+1)+": ");
					for (int nn=0;nn<iVarNo+1;nn++)
					{
						if (nn>0 && nn<iVarNo)
							Console.Write(" + ");
						if (nn<iVarNo)
							Console.Write("X"+(nn+1)+"*("+mat[k,nn].ConvertToString()+")");
						else 
							Console.Write(" = "+mat[k,nn].ConvertToString()  );
					}	
				}
				Console.Write("\n\n");
				if (i==iVarNo) continue;
				for(int j=0;j<iVarNo+1;j++,cnt++)
				{
					if (j>0 && j<iVarNo)
						Console.Write("+");
					if (j<iVarNo)
						Console.Write("X"+cnt+"*");
					else
						Console.Write("=");
					str=Console.ReadLine();
					if ( str.Length==0 )
						str="0";
					mat[i,j]=new Fraction(str);
				}
			}
			Console.WriteLine( "\naugmented matrix:\n"+ mat.ConvertToString() );
			MATRIX ech=mat.EchelonForm();
			Console.WriteLine("\nechelon form:\n"+ech.ConvertToString());
			ech=mat.ReducedEchelonForm();
			for (i=0;i<iVarNo;i++)
				if ( ech[i,i]==0 )		// if not unique solution
				{
					Console.WriteLine("The given system of Equation does not produce any unique solution");
					return;
				}
			Console.Write("\nSolution for the given system:\n");
			for (i=0;i<iVarNo;i++)
					Console.Write("X"+(i+1)+"="+ech[i,iVarNo].ConvertToString()+"\t");
		}	//end EquationSolver()
	}	// end class MatrixApplication*/





}
