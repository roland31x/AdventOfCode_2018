import java.io.File

fun setup() {
    for (i in 1..25) {
        val path = "src/day$i.kt"

        val content = """
import FetchAPI.GetInput
            
class Day$i {
    
    val input: List<String> = GetInput($i)

    fun Solve() {
        println("Part 1: " + part1(input))
        println("Part 2: " + part2(input))
    }

    fun part1(input: List<String>) : Int {
        var ans = 0

        return ans
    }

    fun part2(input: List<String>) : Int {
        var ans = 0

        return ans
    }
}
            
        """

        File(path).writeText(content)
    }
}

setup()