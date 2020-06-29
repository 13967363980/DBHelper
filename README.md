# DBHelper
支持Oracle、MSSQL、MySQL、SQLite四种数据库，配套Model生成器，关系实体映射

实现了分页查询

使用了反射

没有实现缓存

通过 SqlString 类提供非常方便的参数化查询

为什么自己写DBHelper，而不使用Entity Framework、Dapper等ORM框架？我更喜欢使用原生SQL，然后把查询结果映射到实体类。

为什么不使用Dapper？Dapper还是有点复杂，该DBHelper致力于使用尽可能少的方法或接口、尽可能简单的规则或语法实现增删改查。

我的邮箱：651029594@qq.com
