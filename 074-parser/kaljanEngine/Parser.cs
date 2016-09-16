using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BPSimplified.Lib;
using System.Drawing;
using BPSimplified;
using System.IO;

namespace _074_Parser.kaljanEngine
{/*
	class Parser
	{
		int countOfPatterns = 11;

//		Bitmap bitmap = null;

		int av_ImageHeight = 0;
		int av_ImageWidth = 0;

		private NeuralNetwork<string> neuralNetwork = null;

		private Dictionary<string , double[]> TrainingSet = null;

		public Parser( )
		{
			//this.bitmap = bit;
			GenerateTrainingSet();
			CreateNeuralNetwork();


			string[] Images = Directory.GetFiles( _074_Parser.Properties.Settings.Default.pathToPatterns , "*.bmp" );
			this.countOfPatterns = Images.Length;

			av_ImageHeight = 0;
			av_ImageWidth = 0;

			foreach ( string s in Images )
			{
				Bitmap Temp = new Bitmap( s );
				av_ImageHeight += Temp.Height;
				av_ImageWidth += Temp.Width;
				Temp.Dispose();
			}
			av_ImageHeight /= countOfPatterns;
			av_ImageWidth /= countOfPatterns;

		}

		private void buttonTrain_Click( object sender , EventArgs e )
		{
			neuralNetwork.Train();
		}

		private void GenerateTrainingSet( )
		{

			string[] Patterns = Directory.GetFiles( _074_Parser.Properties.Settings.Default.pathToPatterns , "*.bmp" );

			countOfPatterns = Patterns.Count();

			TrainingSet = new Dictionary<string , double[]>( Patterns.Length );
			foreach ( string s in Patterns )
			{
				using ( Bitmap Temp = new Bitmap( s ) )
				{
					TrainingSet.Add( Path.GetFileNameWithoutExtension( s ) ,
						ImageProcessing.ToMatrix( Temp , av_ImageHeight , av_ImageWidth ) );
				}
			}

		}

		void saveNet( )
		{
			neuralNetwork.SaveNetwork( _074_Parser.Properties.Settings.Default.PathToNet );
		}
		void loadNet( )
		{
			neuralNetwork.LoadNetwork( _074_Parser.Properties.Settings.Default.PathToNet );
		}

		private void CreateNeuralNetwork( )
		{
			if ( TrainingSet == null )
				throw new Exception( "Unable to Create Neural Network As There is No Data to Train.." );

			neuralNetwork = new NeuralNetwork<string>
				( new BP1Layer<string>( av_ImageHeight * av_ImageWidth , countOfPatterns ) , TrainingSet );

			neuralNetwork.MaximumError = 1.1;
		}



		void trainNetwork( )
		{
			try
			{
				neuralNetwork.LoadNetwork( _074_Parser.Properties.Settings.Default.PathToNet );
			}
			catch ( System.IO.FileNotFoundException )
			{
				CreateNeuralNetwork();
				if ( neuralNetwork.Train() )
				{
					neuralNetworkTrained = true;
					neuralNetwork.SaveNetwork( _074_Parser.Properties.Settings.Default.PathToNet );
				}
			}
		}

		bool neuralNetworkTrained = false;

		public string getNumberFromBitmap( Bitmap bit )
		{
			if ( neuralNetworkTrained == false )
				trainNetwork();

			string MatchedHigh = "?" , MatchedLow = "?";
			double OutputValueHight = 0 , OutputValueLow = 0;

			double[] input = ImageProcessing.ToMatrix( bit ,
				av_ImageHeight , av_ImageWidth );

			neuralNetwork.Recognize( input , ref MatchedHigh , ref OutputValueHight ,
				ref MatchedLow , ref OutputValueLow );

			return MatchedHigh;
		}
	}
}

	*/

	class Parser
	{
		//Neural Network Object With Output Type String
		private NeuralNetwork<string> neuralNetwork = null;

		//Data Members Required For Neural Network
		private Dictionary<string , double[]> TrainingSet = null;
		private int av_ImageHeight = 0;
		private int av_ImageWidth = 0;
		private int NumOfPatterns = 0;

		double maxError = 0.1;

		public Parser( )
		{
			InitializeSettings();

			GenerateTrainingSet();
			CreateNeuralNetwork();
			trainNetwork();
		}
		private void InitializeSettings( )
		{
			string[] Images = Directory.GetFiles( _074_Parser.Properties.Settings.Default.pathToPatterns , "*.bmp" );
			NumOfPatterns = Images.Length;

			av_ImageHeight = 0;
			av_ImageWidth = 0;

			foreach ( string s in Images )
			{
				Bitmap Temp = new Bitmap( s );
				av_ImageHeight += Temp.Height;
				av_ImageWidth += Temp.Width;
				Temp.Dispose();
			}
			av_ImageHeight /= NumOfPatterns;
			av_ImageWidth /= NumOfPatterns;
		}

		private void GenerateTrainingSet( )
		{
			string[] Patterns = Directory.GetFiles( _074_Parser.Properties.Settings.Default.pathToPatterns , "*.bmp" );

			TrainingSet = new Dictionary<string , double[]>( Patterns.Length );
			foreach ( string s in Patterns )
			{
				Bitmap Temp = new Bitmap( s );
				TrainingSet.Add( Path.GetFileNameWithoutExtension( s ) ,
					ImageProcessing.ToMatrix( Temp , av_ImageHeight , av_ImageWidth ) );
				Temp.Dispose();
			}
		}


		private void CreateNeuralNetwork( )
		{
			if ( TrainingSet == null )
				throw new Exception( "Unable to Create Neural Network As There is No Data to Train.." );
			try
			{
				neuralNetwork.LoadNetwork( _074_Parser.Properties.Settings.Default.PathToNet );
			}
			catch
			{
				neuralNetwork = new NeuralNetwork<string>
						( new BP2Layer<string>( av_ImageHeight * av_ImageWidth , 169 , NumOfPatterns ) , TrainingSet );

				neuralNetwork.MaximumError = maxError;
				neuralNetwork.Train();
				neuralNetwork.SaveNetwork( _074_Parser.Properties.Settings.Default.PathToNet );
			}
		}

		void trainNetwork( )
		{
			neuralNetwork.Train();
		}

		public string recognize( Bitmap bmp )
		{
			//bmp.Save( "img" + DateTime.Now.Millisecond + ".bmp" );
			trainNetwork();
			string MatchedHigh = "?" , MatchedLow = "?";
			double OutputValueHight = 0 , OutputValueLow = 0;

			double[] input = ImageProcessing.ToMatrix( bmp ,
				av_ImageHeight , av_ImageWidth );

			neuralNetwork.Recognize( input , ref MatchedHigh , ref OutputValueHight ,
				ref MatchedLow , ref OutputValueLow );

			return MatchedHigh;
		}
	}
}