package FetchAPI

import java.io.File
import java.net.HttpURLConnection
import java.net.URL

fun FetchInput(day: Int, year: Int = 2018) {
    val currentDir = System.getProperty("user.dir")
    println("Current working directory: $currentDir")
    val session = File("session.txt").readText().trim()

    val url = URL("https://adventofcode.com/$year/day/$day/input")
    val connection = url.openConnection() as HttpURLConnection

    connection.setRequestProperty("User-Agent", "Mozilla/5.0")
    connection.setRequestProperty("Cookie", "session=$session")

    connection.connect()

    val responseCode = connection.responseCode

    val content = connection.inputStream.bufferedReader().use {
        it.readText()
    }

    connection.disconnect()

    val path = "inputs/day$day.txt"

    File(path).writeText(content)
}

fun GetInput(day: Int, year: Int = 2018): List<String> {
    val path = "inputs/day$day.txt"

    if (!File(path).exists()) {
        FetchInput(day, year)
    }

    var input = File(path).readText().split('\n')

    input = input.dropLast(1)

    return input
}