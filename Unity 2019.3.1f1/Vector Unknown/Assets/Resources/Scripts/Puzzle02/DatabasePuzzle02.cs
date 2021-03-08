using UnityEngine;

public struct transformMatrix
{
    public int[] values;
}

public class DatabasePuzzle02 : MonoBehaviour
{
    private GameControllerPuzzle02 GCP02;
    
    public transformMatrix[] transforms0;
    public transformMatrix[] transforms1;
    public transformMatrix[] transforms2;
    public transformMatrix[] transforms3;
    public transformMatrix[] transforms4;
    public transformMatrix[] transforms5;

    transformMatrix t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, t18, t19, t20, t21, t22, t23, t24;

    public int[,] tragetMatracies = new int[,] { {15, 15}, {-21, 24}, {-30 ,51}, {-7, 72}, {9, 47}, {33, 35} }; 

    public void InitDatabase()
    {
        Debug.Log("Connect GameController of Puzzle02");
        GCP02 = GetComponent<GameControllerPuzzle02>();

        refreshDatabase();
    }
  
    public void refreshDatabase()
    {
        t1.values = new int[]  { 15, 0, 0, 15 };    //correct
        t2.values = new int[]  { -15, 0, 0, -15 };
        t3.values = new int[]  { 0, 0, 15, 15 };
        t4.values = new int[]  { 15, 15, 15, 15 };
        //question 2 uses (1,-1)
        t5.values = new int[] { 0, 21, 24, 0 };
        t6.values = new int[] { -20, 1, 1, -23 };
        t7.values = new int[] { -21, 0, 0, -24 };   //correct
        t8.values = new int[] { -20, -1, -1, -23 };
        //question 3 uses (1, 0)
        t9.values = new int[] { -30, -30, 51, 51 };     //correct
        t10.values = new int[] { 51, 51, 30, 30 };
        t11.values = new int[] { 51, 51, -30, -30 };
        t12.values = new int[] { 15, 15, 25, 26 };
        //question 4 uses (0, 1)
        t13.values = new int[] { -7, 7, 72, 72 };
        t14.values = new int[] { -7, -7, 72, 72 };     //correct 
        t15.values = new int[] { 7, 7, -72, 72 };
        t16.values = new int[] { 7, 7, 72, -72 };
        //question 5 uses (1, 1)
        t17.values = new int[] { 6, 3, 40, 7 };     //correct
        t18.values = new int[] { 9, 9, 20, 27 };
        t19.values = new int[] { 9, 1, -47, -1 };
        t20.values = new int[] { 3, 6, 7, 40 };
        //question 6
        t21.values = new int[] { -30, 3, -30, 5 };      //correct
        t22.values = new int[] { 15, 15, 30, 5 };
        t23.values = new int[] { -33, 0, 0, 35 };
        t24.values = new int[] { 30, -3, 30, -5 };      //correct

        transforms0 = new transformMatrix[] { t1, t2, t3, t4 };
        transforms1 = new transformMatrix[] { t5, t6, t7, t8 };
        transforms2 = new transformMatrix[] { t9, t10, t11, t12 };
        transforms3 = new transformMatrix[] { t13, t14, t15, t16 };
        transforms4 = new transformMatrix[] { t17, t18, t19, t20 };
        transforms5 = new transformMatrix[] { t21, t22, t23, t24 };
    }

    // multiply given matrix by given vector and return result
    public int[] calculation(int[] vector, int[] matrix)
    {
        int[] result = { 0, 0 };

        result[0] = matrix[0] * vector[0] + matrix[1] * vector[1];
        result[1] = matrix[2] * vector[0] + matrix[3] * vector[1];

        refreshDatabase();

        return result;
    }
}
