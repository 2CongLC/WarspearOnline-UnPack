' https://warspear-online.com/en/
' https://store.steampowered.com/app/326360

Imports System
Imports System.Collections
Imports System.IO
Imports System.IO.Compression
Imports System.Linq.Expressions
Imports System.Runtime
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Text.RegularExpressions

Module Program

    Public br As BinaryReader
    Public input As String

    Sub Main(args As String())

        If args.Count = 0 Then
            Console.WriteLine("Tool UnPack - 2CongLC.vn :: 2024")
        Else
            input = args(0)
        End If

        Dim p As String = Nothing
        If IO.File.Exists(input) Then

            br = New BinaryReader(File.OpenRead(input))

            Dim signature As String = New String(br.ReadChars(4)) ' Offset = 0, Length = 4
            If signature <> "MDPK" Then
                Console.WriteLine("This is not Warspear Online's pak file.")
            End If
            Dim version As Int16 = br.ReadInt16 ' Offset = 4, Length = 2
            Dim count As Int32 = br.ReadInt32 ' Offset = 6, Length = 4
            Dim [date] As String = New String(br.ReadChars(10))
            br.BaseStream.Position += 6

            Dim subfiles As New List(Of FileData)()
            For i As Int32 = 0 To count - 1
                subfiles.Add(New FileData)
            Next

            p = Path.GetDirectoryName(input) & "\" & Path.GetFileNameWithoutExtension(input)

            For Each fd As FileData In subfiles

                Console.WriteLine("File Offset : {0} - File Size : {1} - File Name : {2}", fd.offset, fd.size, fd.name)

                Directory.CreateDirectory(p & "\" & Path.GetDirectoryName(fd.name))
                br.BaseStream.Position = fd.offset
                Dim buffer As Byte() = br.ReadBytes(fd.size)
                Dim fp As String = p & "\" & fd.name.Replace("/", "\")

                Using bw As New BinaryWriter(File.Create(fp))
                        bw.Write(buffer)
                    End Using

            Next

            br.Close()
                Console.WriteLine("unpack done!!!")
            End If
            Console.ReadLine()
    End Sub


    Public Class FileData
        Public offset As Int32 = br.ReadInt32
        Public size As Int32 = br.ReadInt32
        Public unknow1 As Int32 = br.ReadInt32
        Public unknow2 As Byte = br.ReadByte
        Public name As String = New String(br.ReadChars(55)).TrimEnd(vbNullChar)
    End Class

End Module
