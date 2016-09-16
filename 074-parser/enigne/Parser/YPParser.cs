using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Threading;
using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace _074_Parser.Enigne.Parser
{
	class YPParser
	{
		int cConst = 180;

		public event EventHandler HaveRow;

		public void GetList( )
		{
			ParserValuesList.FilterOptions = new List<ParserValuesList.OneRow>();
			WebClient wc = new WebClient();
			string page = wc.DownloadString( "http://www.spr.ru/all/" );
			int index = page.IndexOf( "<select name='raion_url'>" );
			index += 25;
			int stop = page.IndexOf( "</select><input type='submit'" );
			page = page.Substring( index , stop - index );
			string[] parse = new string[1]
                {
                    "<option value='"
                };
			string[] t1 = page.Split( parse , StringSplitOptions.None );
			foreach ( var item in t1 )
			{
				if ( item != "" )
				{
					ParserValuesList.OneRow oneRow = new ParserValuesList.OneRow();
					int a = item.IndexOf( "'" );
					oneRow.URL = "http://www.spr.ru/" + item.Substring( 0 , a ) + "/";
					index = item.IndexOf( ">" );
					stop = item.IndexOf( "<" );
					oneRow.Name = item.Substring( index + 1 , stop - index - 1 );
					ParserValuesList.FilterOptions.Add( oneRow );
				}
			}
			int i = 0;
		}

		public List<string> ParseURIs( string row )
		{
			List<string> temp = new List<string>();

			bool isEnd = false;

			while ( !isEnd )
			{
				int index = row.IndexOf( ParserValuesList.SelectedFilter.URL );
				if ( index == -1 )
				{
					isEnd = true;
					break;
				}
				row = row.Remove( 0 , index );
				index = row.IndexOf( "\"" );
				string url = row.Substring( 0 , index );
				temp.Add( url );
				row = row.Remove( 0 , index );
			}

			int i = 0;

			while ( i < temp.Count() )
			{
				string a = temp[i];
				if ( a[a.Count() - 1] != '/' )
				{
					temp.Remove( a );
				}
				else
				{
					i++;
				}
			}

			return temp;
		}

		public List<string> ParseIneerURL( string row )
		{
			List<string> temp = new List<string>();

			bool isEnd = false;

			while ( !isEnd )
			{
				int index = row.IndexOf( ParserValuesList.SelectedFilter.URL );
				if ( index == -1 )
				{
					isEnd = true;
					break;
				}
				row = row.Remove( 0 , index );
				index = row.IndexOf( "\"" );
				string url = row.Substring( 0 , index );
				if ( temp.IndexOf( url ) == -1 )
					temp.Add( url );
				row = row.Remove( 0 , index );
			}

			int i = 0;

			while ( i < temp.Count() )
			{
				string a = temp[i];
				if ( a[a.Count() - 1] != 'l' )
				{
					temp.Remove( a );
				}
				else
				{
					i++;
				}
			}

			return temp;
		}

		public void ParseRow( string data )
		{
			DataRow dr = new DataRow();
			int index = data.IndexOf( "<H1>" );
			data = data.Remove( 0 , index + 4 );
			index = data.IndexOf( "</H1>" );
			dr.Name = data.Substring( 0 , index );

			index = data.IndexOf( "Рубрика" );
			data = data.Remove( 0 , index + 7 );
			index = data.IndexOf( "<A" );
			data = data.Remove( 0 , index + 2 );
			index = data.IndexOf( ">" );
			data = data.Remove( 0 , index + 1 );
			index = data.IndexOf( "<" );
			dr.Rubric = data.Substring( 0 , index );
			data = data.Remove( 0 , index + 1 );


			index = data.IndexOf( "Адрес" );
			data = data.Remove( 0 , index + 7 );
			index = data.IndexOf( "<A" );
			data = data.Remove( 0 , index + 2 );
			index = data.IndexOf( ">" );
			data = data.Remove( 0 , index + 1 );
			index = data.IndexOf( "<" );
			dr.Adress = data.Substring( 0 , index );
			data = data.Remove( 0 , index + 1 );

			index = data.IndexOf( "Справочник" );
			data = data.Remove( 0 , index + 7 );
			index = data.IndexOf( "<A" );
			data = data.Remove( 0 , index + 2 );
			index = data.IndexOf( ">" );
			data = data.Remove( 0 , index + 1 );
			index = data.IndexOf( "<" );
			dr.Sprav = data.Substring( 0 , index );
			data = data.Remove( 0 , index + 1 );

			index = data.IndexOf( "Улица, дом" );
			data = data.Remove( 0 , index + 7 );
			index = data.IndexOf( "view-font" );
			data = data.Remove( 0 , index + 9 );
			index = data.IndexOf( ">" );
			data = data.Remove( 0 , index + 1 );
			index = data.IndexOf( "<" );
			dr.Adress2 = data.Substring( 0 , index );

			data = data.Remove( 0 , index + 1 );


			index = data.IndexOf( "Телефон" );
			data = data.Remove( 0 , index + 7 );
			index = data.IndexOf( "view-font" );
			data = data.Remove( 0 , index + 9 );
			index = data.IndexOf( ">" );
			data = data.Remove( 0 , index + 1 );
			index = data.IndexOf( "<" );
			dr.Phone = data.Substring( 0 , index );
			data = data.Remove( 0 , index + 1 );

			index = data.IndexOf( "src=\"" );
			data = data.Remove( 0 , index + 5 );
			index = data.IndexOf( '"' );
			try
			{
				string URL = data.Substring( 0 , index );
				URL = URL.Replace( "&amp;" , "&" );
				//грузим картинку на диск
				getData( URL );

				//Запускаем процесс определения
				dr.Phone += BmpToString();
				File.Delete( AppDomain.CurrentDomain.BaseDirectory + "img.jpg" );
				dr.Phone = dr.Phone.Replace( "&nbsp;" , "" );
			}
			catch { }
			Data.DATA.Add( dr );
		}

		private void AddRow( DataRow dr )
		{
			Dispatcher.CurrentDispatcher.BeginInvoke( DispatcherPriority.Render , new Action( ( ) =>
				{
					Data.DATA.Add( dr );
					if ( HaveRow != null )
						HaveRow( this , new EventArgs() );
				} ) );

		}


		private string BmpToString( )
		{
			//Загружаем картинку в битмап
			Bitmap bi;
			bi = new Bitmap( AppDomain.CurrentDomain.BaseDirectory + "img.jpg" );
			//using (MemoryStream outStream = new MemoryStream())
			//{
			//    BitmapEncoder enc = new BmpBitmapEncoder();
			//    enc.Frames.Add(BitmapFrame.Create(bi));
			//    enc.Save(outStream);
			//    System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

			//    // return bitmap; <-- leads to problems, stream is closed/closing ...
			//    bmp = new Bitmap(bitmap);
			//}
			string answer = "";

			//Делим картинку на отдельные символы
			List<Bitmap> temp = new List<Bitmap>();

			//Хранение столбцов для удаления
			List<int> columnsToRemove = new List<int>();

			//Рассчитываем столбцы, которые надо удалить
			for ( int i = 0 ; i < bi.Width ; i++ )
			{
				bool ignored = false;
				for ( int j = 0 ; j < bi.Height ; j++ )
				{
					Color c = bi.GetPixel( i , j );
					if ( c.R < cConst && c.G < cConst || c.B < cConst )
					{
						ignored = true;
						break;
					}
				}
				if ( ignored == false )
				{
					columnsToRemove.Add( i );
				}
			}

			//Заполняем список картинок-символов
			int a = 0;
			foreach ( var item in columnsToRemove )
			{
				if ( item > a + 1 )
				{
					Bitmap newBMP = new Bitmap( item - a , bi.Height );
					for ( int i = a ; i < item ; i++ )
					{
						for ( int j = 0 ; j < bi.Height ; j++ )
						{
							newBMP.SetPixel( i - a , j , bi.GetPixel( i , j ) );
						}
					}
					temp.Add( newBMP );
					a = item;
				}
				else
					a = item;
			}

			//Удаляем картинки, которые содержат тире

			List<int> trash = new List<int>();
			foreach ( var item in temp )
			{
				if ( item.Width == 6 )
				{
					trash.Add( temp.IndexOf( item ) );
				}
			}
			foreach ( var item in trash )
			{
				//temp.RemoveAt( item );
			}

			kaljanEngine.Parser worker = new kaljanEngine.Parser();
			//Запускаем цикл посимвольного перевода
			foreach ( var item in temp )
			{
				int id = temp.IndexOf( item );
				answer += worker.recognize( new Bitmap( item , new Size( item.Width * 2 , item.Height * 2 ) ) );
				// DetectImage( item , id );
			}
			bi.Dispose();
			return answer;
		}

		public void getData( string url )
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create( url );

			request.MaximumAutomaticRedirections = 200;
			request.MaximumResponseHeadersLength = 200;
			request.Credentials = CredentialCache.DefaultCredentials;
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();

			Stream receiveStream = response.GetResponseStream();
			List<byte> arrOfBytes = new List<byte>();
			int a = 0;
			while ( ( a = receiveStream.ReadByte() ) >= 0 )
			{
				arrOfBytes.Add( (byte)a );
			}

			System.IO.File.WriteAllBytes( AppDomain.CurrentDomain.BaseDirectory + "img.jpg" , arrOfBytes.ToArray() );

			response.Close();
			receiveStream.Close();
		}



		private string DetectImage( Bitmap bmp , int a )
		{
			string answer = "0";

			int w = bmp.Width;
			int h = bmp.Height;
			Color[] r1 = new Color[h];
			Color[] r2 = new Color[h];
			Color[] r3 = new Color[h];
			//Заполняем массивы столбцов. Берем первый, стоблец посредине, и последний
			for ( int i = 0 ; i < h ; i++ )
			{
				r1[i] = bmp.GetPixel( 1 , i );
				r2[i] = bmp.GetPixel( w / 2 , i );
				r3[i] = bmp.GetPixel( w - 1 , i );
			}
			//заполняем переменные для характерестического описания полученнх столбцов.
			//Для поиска используем условие цвета пикселя, если черный, то плюсуем к переменной
			//считаем количество черных пикселей в каждом из столбцов
			int c1 = 0;
			int c2 = 0;
			int c3 = 0;
			for ( int i = 0 ; i < h ; i++ )
			{
				if ( r1[i].R < cConst && r1[i].G < cConst && r1[i].B < cConst )
					c1++;
			}
			for ( int i = 0 ; i < h ; i++ )
			{
				if ( r2[i].R < cConst && r2[i].G < cConst && r2[i].B < cConst )
					c2++;
			}
			for ( int i = 0 ; i < h ; i++ )
			{
				if ( r3[i].R < cConst && r3[i].G < cConst && r3[i].B < cConst )
					c3++;
			}

			//Проверяем по условиям и пытаемся определить символ
			if ( c1 == 2 && c2 == 2 && c3 == 2 )
				return CheckSeven( bmp , a );
			else if ( c1 == 1 && c2 == 1 && c3 == 14 )
				return "1";
			else if ( c1 == 5 && c2 == 3 && c3 == 2 )
				return "2";
			else if ( c1 == 4 && c2 == 3 && c3 == 5 )
				return "3";
			else if ( c1 == 2 && c2 == 3 && c3 == 2 )
				return "4";
			else if ( c1 == 10 && c2 == 4 && c3 == 7 )
				return "5";
			else if ( c1 == 9 && c2 == 3 && c3 == 7 )
				return "6";
			else if ( c1 == 2 && c2 == 4 && c3 == 1 )
				return CheckSeven( bmp , a );
			else if ( c1 == 8 && c2 == 4 && c3 == 8 )
				return "8";
			else if ( c1 == 7 && c2 == 3 && c3 == 1 )
				return "9";
			else if ( c1 == 10 && c2 == 2 && c3 == 10 )
				return "0";

			return answer;
		}

		private string CheckSeven( Bitmap bmp , int id )
		{
			int w = bmp.Width;
			int h = bmp.Height;
			Color[] r1 = new Color[h];
			Color[] r2 = new Color[h];
			Color[] r3 = new Color[h];
			for ( int i = 0 ; i < w ; i++ )
			{
				r1[i] = bmp.GetPixel( i , 1 );
				r2[i] = bmp.GetPixel( i , h / 2 );
				r3[i] = bmp.GetPixel( i , h - 1 );
			}
			//make character
			int c1 = 0;
			int c2 = 0;
			int c3 = 0;
			for ( int i = 0 ; i < h ; i++ )
			{
				if ( r1[i].R < cConst && r1[i].G < cConst && r1[i].B < cConst )
					c1++;
			}
			for ( int i = 0 ; i < h ; i++ )
			{
				if ( r2[i].R < cConst && r2[i].G < cConst && r2[i].B < cConst )
					c2++;
			}
			for ( int i = 0 ; i < h ; i++ )
			{
				if ( r3[i].R < cConst && r3[i].G < cConst && r3[i].B < cConst )
					c3++;
			}

			if ( c1 == 12 && c2 == 5 && c3 == 5 )
				return "7";
			else if ( c1 == 8 && c2 == 8 && c3 == 8 )
				return "1";

			return "";

		}

		private string OneOrSeven( Bitmap bmp )
		{
			string answer = "";
			int w = bmp.Width;
			int h = bmp.Height;

			for ( int i = 0 ; i < h ; i++ )
			{
				if ( bmp.GetPixel( w - 2 , i ).B < cConst )
				{
					int cc = 0;
					for ( int j = 0 ; j < w ; j++ )
					{
						if ( bmp.GetPixel( j , i ).B < cConst )
							cc++;
					}
					if ( cc > w / 2 )
						return "7";
					else
						return "1";
				}
			}

			return answer;
		}
	}
}
